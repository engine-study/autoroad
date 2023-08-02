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
    private Vector3? onchainPos;
    private Vector3 lastOnchainPos = Vector3.down;
    Vector3 lookVector;
    Quaternion lookRotation;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject moveMarker;
    Vector3 markerPos;
    [SerializeField] private SPAnimatorState walkingState;
    [SerializeField] private SPAnimatorState pushState;

    private System.IDisposable? _disposer;
    private MUDEntity entity;
    private PlayerMUD playerScript;

    float alive = 0f;
    float rotationSpeed = 720f;
    float distance;
    // [SerializeField] private AudioClip[] pushes;
    [SerializeField] private AudioClip[] sfx_bump;
    bool entityReady = false;

    void Awake() {
        entity = GetComponentInParent<MUDEntity>();
        enabled = false;
    }

    public override void Init() {
        base.Init();

        // Debug.Log("Controller Init");

        playerScript = GetComponentInParent<PlayerMUD>();
        entityReady = true;

        playerScript.Position.OnUpdated += ComponentUpdate;

        moveMarker.SetActive(false);

        controller.enabled = false;

        playerTransform = transform;
        moveMarker.transform.parent = playerScript.Player.transform;
        moveMarker.transform.position = playerTransform.position;

        playerTransform.rotation = Quaternion.Euler(0f, Random.Range(0, 4) * 90f, 0f);

        onchainPos = playerScript.Position.Pos;
        SetPositionInstant(playerScript.Position.Pos);
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
    Vector3 moveDest;
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

        moveDest = (Vector3)onchainPos;


        // Vector3 moveDest = (Vector3)_destination;
        float moveDistance = 5f;

        moveDest += Mathf.RoundToInt(Input.GetAxis("Horizontal")) * Vector3.right * moveDistance + Mathf.RoundToInt(Input.GetAxis("Vertical")) * Vector3.forward * moveDistance;
  
        minTime = .75f;

        Vector3 direction = (moveDest - playerTransform.position).normalized;
        MUDEntity e = MUDHelper.GetMUDEntityFromRadius(playerTransform.position + direction + Vector3.up * .25f, .1f);
        PositionComponent otherPosition = (e != null ? e.GetMUDComponent<PositionComponent>() : null);
        bool push = otherPosition != null;

        if (push) {

            Debug.Log("PUSHING");

            Vector3 newPos = new Vector3(Mathf.Round(playerTransform.position.x + direction.x), 0f, Mathf.Round(playerTransform.position.z + direction.z));
            Vector3 pushToPos = new Vector3(Mathf.Round(newPos.x + direction.x), 0f, Mathf.Round(newPos.z + direction.z));

            List<TxUpdate> updates = new List<TxUpdate>();
            updates.Add(TxManager.MakeOptimistic(playerScript.Position, (int)newPos.x, (int)newPos.z));
            updates.Add(TxManager.MakeOptimistic(otherPosition, (int)pushToPos.x, (int)pushToPos.z));
            TxManager.Send<PushFunction>(updates, System.Convert.ToInt32(newPos.x), System.Convert.ToInt32(newPos.z), System.Convert.ToInt32(pushToPos.x), System.Convert.ToInt32(pushToPos.z));

        } else {

            Debug.Log("WALKING");
            markerPos = moveDest;

            List<TxUpdate> updates = new List<TxUpdate>();
            updates.Add(TxManager.MakeOptimistic(playerScript.Position, (int)moveDest.x, (int)moveDest.z));

            TxManager.Send<MoveFromFunction>(updates, System.Convert.ToInt32(moveDest.x), System.Convert.ToInt32(moveDest.z));
        }

        markerPos = moveDest;
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

    public const float MOVE_SPEED = 1.5f;

    //this lerps the character transform
    public void UpdatePosition() {

        if (playerTransform.position != moveDest) {
            Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, moveDest, MOVE_SPEED * Time.deltaTime);
            distance += Vector3.Distance(playerTransform.position, newPosition);

            playerTransform.position = newPosition;
        }

        // Determine the new rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        moveMarker.transform.position = Vector3.Lerp(moveMarker.transform.position, markerPos + Vector3.up * .1f + Vector3.up * Mathf.Sin(Time.time * 5f) * .1f, .2f);

        if (alive > 1f && distance > .5f) {
            distance -= .5f;
            player.Resources.sfx.PlaySound(player.Resources.stepSFX);
        }

    }


    private void ComponentUpdate() {

        if (!entityReady) { return; }
        if (playerScript.Position.UpdateSource == UpdateSource.Revert || playerScript.Position.UpdateType == UpdateType.SetRecord) {
            Debug.Log("Teleporting", this);
            onchainPos = playerScript.Position.Pos;
            SetPositionInstant(playerScript.Position.Pos);
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
        Vector3 direction = playerTransform.position == moveDest ? player.Root.forward : (moveDest - playerTransform.position).normalized;
        Vector3 position = playerTransform.position + direction;

        MUDEntity entityAtMove = GridMUD.GetEntityAt(position);
        MUDEntity terrainAtMove = GridMUD.GetEntityAt(position + Vector3.down);

        // Physics.Raycast(playerTransform.position + Vector3.up * .25f, direction, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        // Debug.Log("Hit: " + (hit.collider ? hit.collider.gameObject.name : "No"));

        //&& (hit.collider.GetComponentInParent<RockComponent>() != null || hit.collider.GetComponentInParent<PlayerComponent>() != null)
        if (entityAtMove) {
            //pushing animation
            // Debug.Log("PUSHING");

            if (playerTransform.position == moveDest) {
                //appear tired because we tried to move but our position didn't change (walking into a wall)
                Debug.Log("Tired", this);
                walkingState.Apply(player.Animator);
                player.Animator.PlayClip("Hit");
                player.Resources.sfx.PlaySound(sfx_bump);
            } else {
                Debug.Log("Pushing", this);
                pushState.Apply(player.Animator);
                player.Animator.PlayClip("Push");
                player.Resources.sfx.PlaySound(sfx_bump);

            }

            // markerPos = hitGrid;

        } else {
            //remember the idle animation actually has walk functionality in the AnimationController
            Debug.Log("Walking", this);
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
    }

}