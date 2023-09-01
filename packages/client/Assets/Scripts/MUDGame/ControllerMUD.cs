#nullable enable
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Unity;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;
using mud.Client;

public class ControllerMUD : SPController {


    public System.Action OnFinishedMove;

    [Header("MUD")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject moveMarker;

    [Header("FX")]
    [SerializeField] private AudioClip[] sfx_bump;

    private System.IDisposable? _disposer;
    private MUDEntity mudEntity;
    private PlayerMUD playerScript;

    private Vector3 onchainPos;
    private Vector3 lastOnchainPos = Vector3.down;
    Vector3 lastPos, lookVector;
    Quaternion lookRotation;
    Vector3 markerPos;
    float alive = 0f;
    float rotationSpeed = 720f;
    float distance;
    int moveDistance = 3;
    // [SerializeField] private AudioClip[] pushes;
    bool init = false;

    void Awake() {
        mudEntity = GetComponentInParent<MUDEntity>();
        enabled = false;
        distance = Random.Range(0f, .25f);
    }

    public override void Init() {
        base.Init();

        // Debug.Log("Controller Init");

        playerScript = GetComponentInParent<PlayerMUD>();
        playerScript.Position.OnUpdated += ComponentUpdate;

        moveMarker.SetActive(false);
        controller.enabled = false;

        playerTransform = transform;
        moveMarker.transform.parent = playerScript.Player.transform;
        moveMarker.transform.position = playerTransform.position;

        playerTransform.rotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f);

        onchainPos = playerScript.Position.Pos;
        SetPositionInstant(playerScript.Position.Pos);

        init = true;

    }

    public void SetPositionInstant(Vector3 newPos) {

        playerTransform.position = newPos;
        moveDest = newPos;
        lastOnchainPos = newPos;

    }

    private void OnDestroy() {
        if (playerScript) { playerScript.Position.OnUpdated -= ComponentUpdate; }

        _disposer?.Dispose();
    }


    public override void ToggleController(bool toggle) {
        base.ToggleController(toggle);

        controller.enabled = false;

        //WE ARE ALWAYS ENABLED, BUT OUR CONTROLLER IS NOT
        //TODO, do not always be enabled
        enabled = true;
    }


    void Update() {

        alive += Time.deltaTime;

        if(!playerScript.Alive) {
            return;
        }

        UpdateInput();
        UpdatePosition();

    }

    float minTime = 0f;
    float transactionWait = 1f;
    float cancelWait = .5f;
    Vector3 moveDest, direction;
    void UpdateInput() {

        if (!hasInit) {
            return;
        }

        if (!player.IsLocalPlayer)
            return;

        minTime -= Time.deltaTime;
        if (minTime > 0f) {
            return;
        }

        //playerTransform.position != _onchainPosition ||
        if (Vector3.Distance(playerTransform.position, moveDest) > .1f) {
            return;
        }

        //update rotation based on mouseInput
        // Determine the new rotation
        Vector3 mouseDir = SPInput.MousePlanePos - playerTransform.position;
        if ( mouseDir.magnitude > .5f) { //playerScript.Actor.ActionState == ActionState.Idle && mouseDir.magnitude > .5f

            playerScript.Animator.IK.SetLook(CursorMUD.LookTarget);
            SetLookRotation(playerTransform.position + mouseDir);

        } else {
            playerScript.Animator.IK.SetLook(null);
        }

        bool noModifiers = !SPInput.ModifierKey;
        bool input = noModifiers && (Mathf.RoundToInt(Input.GetAxis("Horizontal")) != 0 || Mathf.RoundToInt(Input.GetAxis("Vertical")) != 0);

        if (!input)
            return;


        Vector3 movePos = onchainPos + Mathf.RoundToInt(Input.GetAxis("Horizontal")) * Vector3.right * moveDistance + Mathf.RoundToInt(Input.GetAxis("Vertical")) * Vector3.forward * moveDistance;
        direction = (movePos - playerScript.Position.Pos).normalized;
        Vector3 moveTo = playerScript.Position.Pos + direction;

        // MUDEntity e = MUDHelper.GetMUDEntityFromRadius(playerScript.Position.Pos + direction + Vector3.up * .25f, .1f);
        MUDEntity e = GridMUD.GetEntityAt(moveTo);
        MoveComponent moveComponent = e?.GetMUDComponent<MoveComponent>();

        Vector3 moveMinimum = onchainPos + direction;
        if(!MapConfigComponent.OnMap((int)moveMinimum.x, (int)moveMinimum.z)) {
            BoundsComponent.ShowBorder();
            return;
        }
        

        if (moveComponent != null) {

            if(moveComponent.MoveType != MoveType.Push) {
                FailedMove(moveTo);
                return;
            }

            Debug.Log("Push Tx");

            bool weight = false;
            bool obstruction = false;

            List<PositionComponent> positions = new List<PositionComponent>();
            List<Vector3> targets = new List<Vector3>();

            Vector3 pushToPos = moveTo;
            MUDEntity destinationEntity = GridMUD.GetEntityAt(pushToPos);

            while(destinationEntity != null) {

                PositionComponent pos = destinationEntity.GetMUDComponent<PositionComponent>();
                MoveComponent destMoveComponent = destinationEntity.GetMUDComponent<MoveComponent>();
                PlayerComponent player = destinationEntity.GetMUDComponent<PlayerComponent>();

                if(pos == null || destMoveComponent == null || destMoveComponent.MoveType == MoveType.Obstruction || (player == null && !MapConfigComponent.OnMap(pushToPos+direction))) {
                    FailedMove(pushToPos);
                    return;
                } else if(destMoveComponent.MoveType != MoveType.Push) {
                    break;
                }

                pushToPos += direction;

                positions.Add(pos);
                targets.Add(pushToPos);

                destinationEntity = GridMUD.GetEntityAt(pushToPos);
            }

            List<TxUpdate> updates = new List<TxUpdate>();

            //update everyones action and position
            for (int i = positions.Count-1; i >= 0; i--) { 
                updates.Add(ActionsMUD.ActionOptimistic(positions[i].Entity, ActionName.Push, targets[i] + direction));
                updates.Add(ActionsMUD.PositionOptimistic(positions[i].Entity, targets[i]));
            }

            //update our own position
            updates.Add(ActionsMUD.ActionOptimistic(mudEntity, ActionName.Push, moveTo + direction));
            updates.Add(ActionsMUD.PositionOptimistic(mudEntity, moveTo));
            ActionsMUD.ActionTx(mudEntity, ActionName.Push, moveTo, updates);

        } else {

            Debug.Log("Walk TX");
            List<TxUpdate> updates = new List<TxUpdate>() { ActionsMUD.PositionOptimistic(mudEntity, movePos) };
            ActionsMUD.ActionTx(mudEntity, ActionName.Walking, movePos, updates);

        }

        markerPos = moveDest;
        minTime = transactionWait;

    }

    public void FailedMove(Vector3 proposedPosition) {
        
        if(!BoundsComponent.OnBounds((int)proposedPosition.x, (int)proposedPosition.z)) {
            BoundsComponent.ShowBorder();
        }

        Debug.Log("Push Tx Canceled");
        MotherUI.TransactionFailed();
        player?.Animator.PlayClip("Hit");
        minTime = cancelWait;
    }

    public void TeleportMUD(Vector3 position, bool admin = false) {

        moveDest = position;

        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(ActionsMUD.PositionOptimistic(mudEntity, position));

        if(admin) {
            updates.Add(ActionsMUD.ActionOptimistic(mudEntity, ActionName.Teleport, position));
            TxManager.Send<TeleportAdminFunction>(updates, PositionComponent.PositionToTransaction(position));
        } else { 
            ActionsMUD.ActionTx(mudEntity, ActionName.Teleport, position, updates); 
        }
    }

    async UniTask FollowTx(UniTask<bool> tx) {
        bool success = await tx;
        if(success) {

        } else {
            BoundsComponent.ShowBorder();
        }
    }

    public const float MOVE_SPEED = 1f;

    //this lerps the character transform
    public void UpdatePosition() {

        //ROTATE
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        moveMarker.transform.position = Vector3.Lerp(moveMarker.transform.position, markerPos + Vector3.up * .1f + Vector3.up * Mathf.Sin(Time.time * 5f) * .1f, .2f);

        if(playerTransform.position == moveDest) {
            return;
        }

        //MOVE
        // Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, moveDest, MOVE_SPEED * Time.deltaTime);
        distance += Vector3.Distance(playerTransform.position, lastPos);
        // playerTransform.position = newPosition;
     

        //STEP FX
        if (alive > 1f && distance > .5f) {
            distance -= Random.Range(.5f, .75f);
            player.Resources.sfx.PlaySound(player.Resources.stepSFX);
        }

        //DONE walking/pushing
        if(playerTransform.position == moveDest) {
            player?.Animator.PlayClip("Idle");
            OnFinishedMove?.Invoke();
        }

        lastPos = playerTransform.position;

    }

    public void SetLookRotation(Vector3 newLookAt) {
       var _lookY = newLookAt;
        _lookY.y = playerTransform.position.y;

        if (_lookY != playerTransform.position) {
            Vector3 eulerAngles = Quaternion.LookRotation(_lookY - playerTransform.position).eulerAngles;
            lookVector = (_lookY - playerTransform.position).normalized;
            lookRotation = Quaternion.Euler(eulerAngles.x, (int)Mathf.Round(eulerAngles.y / 90) * 90, eulerAngles.z);
        }
    }


    private void ComponentUpdate() {

        if (!init) { return; }
        //|| playerScript.Position.UpdateType == UpdateType.SetRecord
        if (playerScript.Position.UpdateInfo.Source == UpdateSource.Revert ) {
            Debug.Log("Teleporting", this);
            SetPositionInstant(playerScript.Position.Pos);
        } else {
            //UPDATE ROTATION 
            if (playerTransform.position != playerScript.Position.Pos) {
                ((ControllerMUD)playerScript.Controller).SetLookRotation(playerScript.Position.Pos);
            }
        }

        //get the actual onchainposition
        onchainPos = playerScript.Position.Pos;

        //update our moveDestination, we must always observe the current onchain state
        moveDest = playerScript.Position.Pos;

      

        lastOnchainPos = onchainPos;

    }

    void OnDrawGizmos() {

        if (moveDest != Vector3.zero) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerTransform.position + Vector3.up * .25f, playerTransform.position + Vector3.up * .25f + (moveDest - playerTransform.position).normalized);
        }

        if(Application.isPlaying && init) {
            Gizmos.DrawLine(playerScript.Position.Pos, playerScript.Position.Pos + direction);
        }
    }

}