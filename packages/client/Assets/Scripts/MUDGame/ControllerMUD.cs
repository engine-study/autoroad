#nullable enable
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using mudworld;
using IWorld.ContractDefinition;
using mud;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;
using mud;

public class ControllerMUD : SPController
{


    public System.Action OnFinishedMove;
    public bool MovementInput { get { return input; } }

    [Header("Actions")]
    [SerializeField] SPInteract walkAction;
    [SerializeField] SPInteract pushAction;
    [SerializeField] GameObject marker;
    [SerializeField] GameObject good, bad;
    SPInteract currentAction;

    [Header("MUD")]
    [SerializeField] Transform playerTransform;

    [Header("FX")]
    [SerializeField] AudioClip[] sfx_bump;

    [Header("Debug")]
    [SerializeField] PositionSync sync;

    System.IDisposable? _disposer;
    MUDEntity mudEntity;
    PlayerMUD playerScript;

    Vector3 onchainPos => sync.Pos.Pos;
    Vector3 lastOnchainPos = Vector3.down;
    Vector3 lastPos;
    Quaternion lookRotation;
    float alive = 0f;
    float rotationSpeed = 720f;
    float distance;
    int moveDistance = 3;
    // [SerializeField] private AudioClip[] pushes;
    bool init = false;
    Vector3 lastInputDir;

    void Awake()
    {
        mudEntity = GetComponentInParent<MUDEntity>();
        enabled = false;
        distance = Random.Range(0f, .25f);
    }

    public override void Init()
    {
        base.Init();

        // Debug.Log("Controller Init");
        sync = GetComponent<PositionSync>();
        playerScript = GetComponentInParent<PlayerMUD>();

        if (playerScript)
        {
            sync.Pos.OnUpdated += ComponentUpdate;
        }

        controller.enabled = false;

        playerTransform = transform;
        playerTransform.rotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f);

        SetPositionInstant(sync.Pos.Pos);

        if (playerScript.IsLocalPlayer)
        {
            SPActionUI.Instance.SpawnAction(walkAction);
            SPActionUI.Instance.SpawnAction(pushAction);
        }

        marker.SetActive(false);

        init = true;

    }

    public void SetPositionInstant(Vector3 newPos)
    {

        playerTransform.position = newPos;
        moveDest = newPos;
        lastOnchainPos = newPos;
        lastPos = newPos;

    }

    private void OnDestroy()
    {
        if (playerScript) { sync.Pos.OnUpdated -= ComponentUpdate; }

        _disposer?.Dispose();
    }

    public override void ToggleController(bool toggle)
    {
        base.ToggleController(toggle);
        controller.enabled = false;
    }


    void Update()
    {

        alive += Time.deltaTime;

        if (!playerScript.Alive) { return; }

        UpdateInput();
        UpdatePosition();

    }

    void LateUpdate()
    {
        UpdateMarker();
    }

    float minTime = 0f;
    float transactionWait = .25f;
    float cancelWait = 1f;
    Vector3 moveDest, inputDir;
    bool input, wasInputting;
    void UpdateInput()
    {

        if (!hasInit)
        {
            return;
        }

        if (!PlayerMUD.CanInput)
            return;

        minTime -= Time.deltaTime;
        if (minTime > 0f)
        {
            return;
        }

        //playerTransform.position != _onchainPosition ||
        if (Vector3.Distance(playerTransform.position, sync.Pos.Pos) > .5f)
        {
            return;
        }

        bool noModifiers = !SPInput.ModifierKey;
        inputDir = new Vector3(Mathf.RoundToInt(Input.GetAxis("Horizontal")), 0f, Mathf.RoundToInt(Input.GetAxis("Vertical")));
        if (inputDir.x != 0f) inputDir.z = 0f;

        inputDir = SPHelper.IsometricToWorld(inputDir);
        input = noModifiers && (inputDir.x != 0 || inputDir.z != 0);

        if (!input) {
            if (wasInputting) { player.Actor.InputAction(false, false, currentAction); wasInputting = false; }
            return;
        } else if (currentAction && inputDir != lastInputDir) {
            //reset the action if we changed direction
            wasInputting = false;
            player.Actor.InputAction(false, false, currentAction);

        }


        //look to rotation
        Vector3 moveTo = onchainPos + inputDir;
        playerScript.AnimationMUD.Look.SetLookRotation(moveTo);

        //check if pushing or walking
        MUDEntity e = GridMUD.GetEntityAt(moveTo);
        MoveComponent moveComponent = e?.GetMUDComponent<MoveComponent>();

        if (!MapConfigComponent.OnWorld((int)moveTo.x, (int)moveTo.z)) {
            BoundsComponent.ShowBorder();
            FailedMove(sync.Pos.Entity, moveTo);
            return;
        }

        //setup the push or walk
        if(moveComponent == null || moveComponent.MoveType == MoveType.Trap || moveComponent.MoveType == MoveType.None) {
            currentAction = walkAction;
            walkAction.transform.position = moveTo;
        } else {
            bool didPush = CreatePush(onchainPos, inputDir);
            if(didPush) {
                currentAction = pushAction;
                pushAction.transform.position = moveTo;
            } else { 
                FailedMove(sync.Pos.Entity, moveTo);
                return;
            }
        }

        //send our input finally!
        player.Actor.InputAction(!wasInputting, true, currentAction);

        //update last values
        lastInputDir = inputDir;
        wasInputting = true;

        bad.SetActive(false);
        good.SetActive(true);

        if (player.Actor.ActionState != ActionState.Casting)
        {
            Debug.Log("MOVE: " + currentAction.gameObject.name);
            minTime = transactionWait;
            wasInputting = false;
        }

    }

    public static bool CreatePush(Vector3 startPush, Vector3 pushDirection) {

        int weightCount = 0;

        List<PositionComponent> positions = new List<PositionComponent>();
        List<Vector3> targets = new List<Vector3>();
        List<Mover> movers = new List<Mover>();

        Vector3 pushToPos = startPush;
        MUDEntity destinationEntity = GridMUD.GetEntityAt(pushToPos);

        while (destinationEntity != null) {

            PositionComponent pos = destinationEntity.GetMUDComponent<PositionComponent>();
            MoveComponent destMoveComponent = destinationEntity.GetMUDComponent<MoveComponent>();
            WeightComponent w = destinationEntity.GetMUDComponent<WeightComponent>();
            PlayerComponent player = destinationEntity.GetMUDComponent<PlayerComponent>();

            weightCount += w ? w.Weight : 0;

            bool isObstructed = weightCount > 0 || pos == null || destMoveComponent == null || destMoveComponent.MoveType == MoveType.Obstruction;
            bool isOffMap = !MapConfigComponent.OnWorldOrMap(destinationEntity, pushToPos + pushDirection);

            if(destMoveComponent != null && pos != null) {
                Mover newMover = new Mover() {
                    cannotMove = isObstructed,
                    target = pos.Target,
                    weight = w ? w.Weight : 0,
                    moveType = destMoveComponent.MoveType
                };

                movers.Add(newMover);
            }

            if (isObstructed || isOffMap) {
                WeightUI.Instance.ToggleWeights(true, movers);
                return false;
            }
            
            if (destMoveComponent.MoveType != MoveType.Push) {
                break;
            }

            pushToPos += pushDirection;

            positions.Add(pos);
            targets.Add(pushToPos);

            destinationEntity = GridMUD.GetEntityAt(pushToPos);
        }

        WeightUI.Instance.ToggleWeights(true, movers);

        // //update everyones action and position
        // List<TxUpdate> updates = new List<TxUpdate>();
        // for (int i = positions.Count-1; i >= 0; i--) { 
        //     updates.Add(ActionsMUD.ActionOptimistic(positions[i].Entity, ActionName.Push, targets[i] + direction));
        //     updates.Add(ActionsMUD.PositionOptimistic(positions[i].Entity, targets[i]));
        // }

        //update our own position
        // updates.Add(ActionsMUD.PositionOptimistic(mudEntity, moveTo));
        // ActionsMUD.ActionTx(mudEntity, ActionName.Push, moveTo, updates);

        return true;
    }

    void UpdateMarker() {

        marker.SetActive((input || minTime > 0f) && currentAction != null && sync.Moving == false);

        if (!marker.activeInHierarchy) return;

        marker.transform.position = transform.position + inputDir;
        marker.transform.rotation = Quaternion.identity;

    }

    public void FailedMove(MUDEntity e, Vector3 proposedPosition) {

        PositionComponent.OnWorldOrMap(e, proposedPosition, true);

        Debug.Log("Move Tx Canceled");
        MotherUI.TransactionFailed();
        player?.Animator.PlayClip("Hit");
        minTime = cancelWait;
        input = false;

        bad.SetActive(true);
        good.SetActive(false);

    }

    public void TeleportMUD(Vector3 position, bool admin = false)
    {

        if (PositionComponent.OnWorld(position, true) == false) { return; }

        moveDest = position;

        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(ActionsMUD.PositionOptimistic(mudEntity, position));


        if (admin)
        {
            updates.Add(ActionsMUD.ActionOptimistic(mudEntity, ActionName.Teleport, position));
            TxManager.Send<TeleportAdminFunction>(updates, PositionComponent.PositionToTransaction(position));
        }
        else
        {
            ActionsMUD.ActionTx(mudEntity, ActionName.Teleport, position, updates);
        }
    }

    async UniTask FollowTx(UniTask<bool> tx)
    {
        bool success = await tx;
        if (success)
        {

        }
        else
        {
            BoundsComponent.ShowBorder();
        }
    }

    public const float MOVE_SPEED = 1f;

    //this lerps the character transform
    public void UpdatePosition()
    {

        //ROTATE
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (playerTransform.position == moveDest)
        {
            return;
        }

        //MOVE
        // Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, moveDest, MOVE_SPEED * Time.deltaTime);
        distance += Vector3.Distance(playerTransform.position, lastPos);
        // playerTransform.position = newPosition;


        //STEP FX
        if (alive > 1f && distance > .5f)
        {
            distance -= Random.Range(.5f, .75f);
            player.Resources.sfx.PlaySound(player.Resources.stepSFX);
        }

        //DONE walking/pushing
        if (playerTransform.position == moveDest)
        {
            player?.Animator.PlayClip("Idle");
            OnFinishedMove?.Invoke();
        }

        lastPos = playerTransform.position;

    }


    private void ComponentUpdate()
    {

        if (!init) { return; }
        //|| sync.Pos.UpdateType == UpdateType.SetRecord
        if (sync.Pos.UpdateInfo.Source == UpdateSource.Revert)
        {
            Debug.Log("Teleporting", this);
            SetPositionInstant(sync.Pos.Pos);
        }
        else
        {
            //UPDATE ROTATION 
            if (playerTransform.position != sync.Pos.Pos)
            {
                playerScript.AnimationMUD.Look.SetLookRotation(sync.Pos.Pos);
            }
        }

        //update our moveDestination, we must always observe the current onchain state
        moveDest = sync.Pos.Pos;

        lastOnchainPos = onchainPos;

    }

    void OnDrawGizmos()
    {

        // if (moveDest != Vector3.zero) {
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawLine(playerTransform.position + Vector3.up * .25f, playerTransform.position + Vector3.up * .25f + (moveDest - playerTransform.position).normalized);
        // }

        // if (inputDir != Vector3.zero) {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(playerTransform.position + Vector3.up * .25f, playerTransform.position + Vector3.up * .25f + inputDir);
        // }

        // if(Application.isPlaying && init) {
        //     Gizmos.DrawLine(sync.Pos.Pos, sync.Pos.Pos + inputDir);
        // }
    }

}