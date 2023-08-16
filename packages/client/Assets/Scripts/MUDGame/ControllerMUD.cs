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
    [SerializeField] private SPAnimatorState walkingState;
    [SerializeField] private SPAnimatorState pushState;

    [Header("FX")]
    [SerializeField] private ParticleSystem fx_teleport;
    [SerializeField] private AudioClip [] sfx_teleport;
    [SerializeField] private AudioClip[] sfx_bump;

    private System.IDisposable? _disposer;
    private MUDEntity entity;
    private PlayerMUD playerScript;

    private Vector3 onchainPos;
    private Vector3 lastOnchainPos = Vector3.down;
    Vector3 lookVector;
    Quaternion lookRotation;
    Vector3 markerPos;
    float alive = 0f;
    float rotationSpeed = 720f;
    float distance;
    int moveDistance = 1;
    // [SerializeField] private AudioClip[] pushes;
    bool init = false;

    void Awake() {
        entity = GetComponentInParent<MUDEntity>();
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

    }

    private void OnDestroy() {
        if (playerScript) {
            playerScript.Position.OnUpdated -= ComponentUpdate;
        }

        _disposer?.Dispose();
    }


    public override void ToggleController(bool toggle) {
        base.ToggleController(toggle);

        controller.enabled = false;

        //WE ARE ALWAYS ENABLED, BUT OUR CONTROLLER IS NOT
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
        if (playerScript.Actor.ActionState == ActionState.Idle && mouseDir.magnitude > .5f) {

            playerScript.Animator.IK.SetLook(CursorMUD.LookTarget);
            Vector3 eulerAngles = Quaternion.LookRotation(mouseDir).eulerAngles;
            lookVector = (mouseDir).normalized;
            lookRotation = Quaternion.Euler(eulerAngles.x, (int)Mathf.Round(eulerAngles.y / 90) * 90, eulerAngles.z);

        } else {
            playerScript.Animator.IK.SetLook(null);
        }

        bool reverseInput = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftShift);
        bool input = !reverseInput && (Mathf.RoundToInt(Input.GetAxis("Horizontal")) != 0 || Mathf.RoundToInt(Input.GetAxis("Vertical")) != 0);

        if (!input)
            return;


        Vector3 movePos = (Vector3)onchainPos;
        movePos += Mathf.RoundToInt(Input.GetAxis("Horizontal")) * Vector3.right * moveDistance + Mathf.RoundToInt(Input.GetAxis("Vertical")) * Vector3.forward * moveDistance;
        direction = (movePos - playerScript.Position.Pos).normalized;
        Vector3 moveTo = playerScript.Position.Pos + direction;

        // MUDEntity e = MUDHelper.GetMUDEntityFromRadius(playerScript.Position.Pos + direction + Vector3.up * .25f, .1f);
        MUDEntity e = GridMUD.GetEntityAt(moveTo);
        MoveComponent moveComponent = e?.GetMUDComponent<MoveComponent>();
        PositionComponent posComponent = e?.GetMUDComponent<PositionComponent>();

        if (moveComponent != null) {

            Debug.Log("PUSHING");

            if(moveComponent.MoveType != MoveType.Push) {
                FailedMove();
                return;
            }

            Vector3 pushToPos = new Vector3(Mathf.Round(moveTo.x + direction.x), 0f, Mathf.Round(moveTo.z + direction.z));

            MUDEntity destinationEntity = GridMUD.GetEntityAt(pushToPos);
            MoveComponent destMoveComponent = destinationEntity?.GetMUDComponent<MoveComponent>();

            if(destMoveComponent != null && (destMoveComponent.MoveType != MoveType.Hole && destMoveComponent.MoveType != MoveType.None)) {
                FailedMove();
                return;
            }

            List<TxUpdate> updates = new List<TxUpdate>();
            updates.Add(TxManager.MakeOptimistic(playerScript.Position, PositionComponent.PositionToOptimistic(moveTo)));
            updates.Add(TxManager.MakeOptimistic(posComponent, PositionComponent.PositionToOptimistic(pushToPos)));
            TxManager.Send<PushFunction>(updates, PositionComponent.PositionToTransaction(moveTo));

            if(!BoundsComponent.OnBounds((int)pushToPos.x, (int)pushToPos.z)) {
                BoundsComponent.ShowBorder();
            }

          
        } else {

            Debug.Log("WALKING");
            markerPos = movePos;

            TxUpdate update = TxManager.MakeOptimistic(playerScript.Position, PositionComponent.PositionToOptimistic(movePos));
            UniTask<bool> tx = TxManager.Send<MoveSimpleFunction>(update, PositionComponent.PositionToTransaction(movePos));

            if(!MapConfigComponent.OnMap((int)movePos.x, (int)movePos.z)) {
                BoundsComponent.ShowBorder();
            }
        }

        markerPos = moveDest;
        minTime = transactionWait;

    }

    public void FailedMove() {
        MotherUI.TransactionFailed();
        player?.Animator.PlayClip("Hit");
        minTime = cancelWait;
    }

    public void TeleportMUD(Vector3 position, bool admin = false) {

        moveDest = position;

        List<TxUpdate> updates = new List<TxUpdate>();
        SetPositionInstant(position);
        updates.Add(TxManager.MakeOptimistic(playerScript.Position, (int)position.x, (int)position.z));
        if(admin) {
            TxManager.Send<TeleportAdminFunction>(updates, System.Convert.ToInt32(position.x), System.Convert.ToInt32(position.z));
        } else {
            TxManager.Send<TeleportFunction>(updates, System.Convert.ToInt32(position.x), System.Convert.ToInt32(position.z));
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
        Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, moveDest, MOVE_SPEED * Time.deltaTime);
        distance += Vector3.Distance(playerTransform.position, newPosition);
        playerTransform.position = newPosition;
     

        //STEP FX
        if (alive > 1f && distance > .5f) {
            distance -= Random.Range(.25f, .5f);
            player.Resources.sfx.PlaySound(player.Resources.stepSFX);
        }

        //DONE walking/pushing
        if(playerTransform.position == moveDest) {
            player?.Animator.PlayClip("Idle");
            OnFinishedMove?.Invoke();
        }


    }


    private void ComponentUpdate() {

        if (!init) { return; }
        //|| playerScript.Position.UpdateType == UpdateType.SetRecord
        if (playerScript.Position.UpdateSource == UpdateSource.Revert ) {
            Debug.Log("Teleporting", this);
            onchainPos = playerScript.Position.Pos;
            SetPositionInstant(playerScript.Position.Pos);
        }

        //good chance we teleported (we guess for now)
        if(playerScript.Position.Pos.x != onchainPos.x && playerScript.Position.Pos.z != onchainPos.z) {
            fx_teleport.Play();
            SetPositionInstant(playerScript.Position.Pos);
            SPAudioSource.Play(playerScript.Position.Pos, sfx_teleport);
            fx_teleport.Play();
        }

        //get the actual onchainposition
        onchainPos = playerScript.Position.Pos;

        //update our moveDestination, we must always observe the current onchain state
        moveDest = playerScript.Position.Pos;

        //our onchain position didn't change, either because we got an update that was already made optimistically
        //or because we literally didn't move
        if (playerScript.Position.UpdateType == UpdateType.SetField && (Vector3)onchainPos == lastOnchainPos) {
            return;
        }

        UpdateState();

    }


    private void UpdateState() {

        //raycast to the world
        RaycastHit hit;
        Vector3 lookDirection = playerTransform.position == moveDest ? player.Root.forward : (moveDest - playerTransform.position).normalized;
        Vector3 position = playerTransform.position + lookDirection;

        MUDEntity entityAtMove = GridMUD.GetEntityAt(position);
        MUDEntity terrainAtMove = GridMUD.GetEntityAt(position + Vector3.down);

        // Physics.Raycast(playerTransform.position + Vector3.up * .25f, lookDirection, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        // Debug.Log("Hit: " + (hit.collider ? hit.collider.gameObject.name : "No"));

        //&& (hit.collider.GetComponentInParent<RockComponent>() != null || hit.collider.GetComponentInParent<PlayerComponent>() != null)
        if (entityAtMove) {
            //pushing animation
            // Debug.Log("PUSHING");

            if (playerTransform.position == moveDest) {
                //appear tired because we tried to move but our position didn't change (walking into a wall)
                Debug.Log("Animation: Tired", this);
                walkingState.Apply(player.Animator);
                player.Animator.PlayClip("Hit");
                player.Resources.sfx.PlaySound(sfx_bump);
            } else {
                Debug.Log("Animation: Pushing", this);
                pushState.Apply(player.Animator);
                player.Animator.PlayClip("Push");
                player.Resources.sfx.PlaySound(sfx_bump);

            }

            // markerPos = hitGrid;

        } else {
            //remember the idle animation actually has walk functionality in the AnimationController
            Debug.Log("Animation: Walking", this);
            player?.Animator.PlayClip("Idle");
            markerPos = moveDest;
        }

        //UPDATE ROTATION
        if (playerScript.Position.UpdateSource != UpdateSource.Revert) {
            var _lookY = moveDest;
            _lookY.y = playerTransform.position.y;

            if (_lookY != playerTransform.position) {
                lookRotation = Quaternion.LookRotation(_lookY - playerTransform.position);
                lookVector = (_lookY - playerTransform.position).normalized;
            }
        }

        if (playerScript.IsLocalPlayer) {
            //stop the player from looking at the cursor when theyre moving
            playerScript.Animator.IK.SetLook(null);
        }

        lastOnchainPos = (Vector3)onchainPos;
    }


    Vector3 ChainPosToEngine(Vector3 chainPos) {
        RaycastHit hit;
        Physics.Raycast(chainPos + Vector3.up * 100f, Vector3.down, out hit, 200f, SPLayers.InvertMaskPlayers, QueryTriggerInteraction.Ignore);
        return hit.point;
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