#nullable enable
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Unity;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;
using mud.Client;

public class ControllerMUD : SPController
{
    private Vector3? _onchainPosition;
    private Vector3 enginePosition;

    Quaternion lookRotation;
    public Transform playerTransform;
    public GameObject moveMarker;
    Vector3 markerPos;

    private System.IDisposable? _disposer;
    private MUDEntity entity;
    private PlayerMUD playerScript;

    float alive = 0f;
    float distance;
    public AudioClip[] pushes;
    public AudioClip[] pops;
    bool entityReady = false;

    void Awake()
    {

        entity = GetComponentInParent<MUDEntity>();

    }

    public override void Init()
    {
        base.Init();

        Debug.Log("Controller Init");

        playerScript = GetComponentInParent<PlayerMUD>();
        entityReady = true;

        playerScript.Position.OnUpdated += PositionUpdate;


        moveMarker.SetActive(false);

        controller.enabled = false;

        playerTransform = transform;
        moveMarker.transform.parent = null;
        moveMarker.transform.position = playerTransform.position;

        playerTransform.rotation = Quaternion.Euler(0f, Random.Range(0,4) * 90f, 0f);


        RevertPosition();

    }

    private void OnDestroy()
    {
        if (playerScript)
        {
            playerScript.Position.OnUpdated -= PositionUpdate;
        }

        _disposer?.Dispose();
    }


    public override void ToggleController(bool toggle)
    {
        base.ToggleController(toggle);

        controller.enabled = false;
        enabled = true;
    }

    private void PositionUpdate()
    {
        if (!entityReady) { return; }

        Vector3 lastOnchainPos = _onchainPosition == null ? playerTransform.position : (Vector3)_onchainPosition;

        _onchainPosition = playerScript.Position.Pos;
        markerPos = (Vector3)_onchainPosition;

        // Debug.Log("New Pos: " + _onchainPosition.ToString());

        UpdateAnimation((Vector3)_onchainPosition);

        // playerTransform.position = (Vector3)_destination;
    }

    // private void SubscribeToCounter(NetworkManager _)
    // {
    // 	_counterSub = ObservableExtensions.Subscribe(CounterTable.OnRecordUpdate().ObserveOnMainThread(), OnIncrement);
    // }

    // private async UniTaskVoid SendIncrementTxAsync()
    // {
    //     try
    //     {
    //         await net.worldSend.TxExecute<IncrementFunction>();
    //     }
    //     catch (Exception ex)
    //     {
    //         // Handle your exception here
    //         Debug.LogException(ex);
    //     }
    // }

    private async UniTaskVoid SendMoveFromTx(int x, int y)
    {
        try
        {
            // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
            await NetworkManager.Instance.worldSend.TxExecute<MoveFromFunction>(x, y);
        }
        catch (System.Exception ex)
        {
            //if our transaction fails, force the player back to their position on the table
            Debug.LogException(ex);
            RevertPosition();
        }
    }

    private async UniTaskVoid SendPushTx(int x, int y, int pushX, int pushY)
    {
        try
        {
            // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
            await NetworkManager.Instance.worldSend.TxExecute<PushFunction>(x, y, pushX, pushY);
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            RevertPosition();
        }
    }

    void RevertPosition()
    {
        Debug.Log("Reverting position");
        playerTransform.position = playerScript.Position.Pos;
        _onchainPosition = playerScript.Position.Pos;
        moveDest = playerScript.Position.Pos;
    }

    // private async UniTaskVoid SendMoveTx(int x, int y)
    // {
    //     try
    //     {
    //         // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
    //         await NetworkManager.Instance.worldSend.TxExecute<MoveFunction>(x, y);
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogException(ex);
    //     }
    // }

    float moveTimeout = .5f;

    void Update()
    {

        alive += Time.deltaTime;

        UpdateInput();
        UpdatePosition();

    }

    Vector3 moveDest;
    void UpdateInput()
    {
        if (!hasInit)
        {
            return;
        }

        if (!player.IsLocalPlayer)
            return;

        //playerTransform.position != _onchainPosition ||
        if(_onchainPosition == null || Vector3.Distance(playerTransform.position, moveDest) > .1f) {
            return;
        }

        bool push = Input.GetKey(KeyCode.LeftShift);

        bool input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (!input)
            return;

        if (_onchainPosition == null)
        {
            moveDest = playerTransform.position;
            moveDest.y = 0f;
        }
        else
        {
            moveDest = (Vector3)_onchainPosition;
        }

        // Vector3 moveDest = (Vector3)_destination;

        if (Input.GetKey(KeyCode.W))
        {
            moveDest += Vector3.forward * 2f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDest += Vector3.left * 2f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDest += Vector3.back * 2f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDest += Vector3.right * 2f;
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        // 	var ray = _camera.ScreenPointToRay(Input.mousePosition);
        // 	if (!Physics.Raycast(ray, out var hit)) return;

        // 	dest = hit.point;


        // }

        // dest = Vector3.Normalize(dest);
        moveDest.x = Mathf.Round(moveDest.x);
        moveDest.y = 0f;
        moveDest.z = Mathf.Round(moveDest.z);

        if (push)
        {

            Debug.Log("PUSHING");

            Vector3 direction = (moveDest - playerTransform.position).normalized;
            moveDest = new Vector3(Mathf.Round(playerTransform.position.x + direction.x), 0f, Mathf.Round(playerTransform.position.z + direction.z));
            Vector3 pushToPos = new Vector3(Mathf.Round(moveDest.x + direction.x), 0f, Mathf.Round(moveDest.z + direction.z));

            _onchainPosition = moveDest;
            SendPushTx(System.Convert.ToInt32(moveDest.x), System.Convert.ToInt32(moveDest.z), System.Convert.ToInt32(pushToPos.x), System.Convert.ToInt32(pushToPos.z)).Forget();
        }
        else
        {

            Debug.Log("WALKING");
            markerPos = moveDest;
            _onchainPosition = moveDest;
            SendMoveFromTx(System.Convert.ToInt32(moveDest.x), System.Convert.ToInt32(moveDest.z)).Forget();
        }

        markerPos = moveDest;
        moveTimeout = .75f;

        UpdateAnimation(moveDest);

        // UIMother.PlayAccept();

    }


    public void UpdatePosition()
    {

        if (_onchainPosition != null && playerTransform.position != _onchainPosition)
        {
            Vector3 newPosition = Vector3.MoveTowards(playerTransform.position, _onchainPosition.Value, 2.5f * Time.deltaTime);
            distance += Vector3.Distance(playerTransform.position, newPosition);

            playerTransform.position = newPosition;

        }

        // Determine the new rotation
        var newRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 720f * Time.deltaTime);
        transform.rotation = newRotation;

        moveMarker.transform.position = Vector3.Lerp(moveMarker.transform.position, markerPos + Vector3.up * .1f + Vector3.up * Mathf.Sin(Time.time * 5f) * .1f, .2f);

        if (alive > 1f && distance > 1f)
        {
            distance -= 1f;
            player.resources.sfx.PlaySound(player.resources.stepSFX);
        }

    }

    void UpdateAnimation(Vector3 toPosition)
    {


        RaycastHit hit;
        Vector3 direction = (toPosition - playerTransform.position).normalized;
        Physics.Raycast(playerTransform.position + Vector3.up * .25f, direction, out hit, 1f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        Debug.Log("Hit: " + (hit.collider ? hit.collider.gameObject.name : "No"));

        if (playerTransform.position == toPosition)
        {

            player?.Animator.PlayClip("Tired");

        }
        else if (hit.collider && (hit.collider.GetComponentInParent<RockComponent>() != null || hit.collider.GetComponentInParent<PlayerComponent>() != null))
        {

            // Debug.Log("PUSHING");

            Vector3 hitGrid = new Vector3(Mathf.Round(hit.point.x), 0f, Mathf.Round(hit.point.z));
            Vector3 pushSpot = new Vector3(Mathf.Round(hitGrid.x + direction.x), 0f, Mathf.Round(hitGrid.z + direction.z));

            player?.Animator.PlayClip("Push");
            markerPos = hitGrid;


        }
        else
        {

            // Debug.Log("WALKING");
            player?.Animator.PlayClip("Idle");
            markerPos = toPosition;

        }

        toPosition.y = ChainPosToEngine(toPosition).y;
        enginePosition = toPosition;

        var _lookY = toPosition;
        _lookY.y = playerTransform.position.y;

        if (_lookY != playerTransform.position)
        {
            lookRotation = Quaternion.LookRotation(_lookY - playerTransform.position);
        }
    }


    Vector3 ChainPosToEngine(Vector3 chainPos)
    {
        RaycastHit hit;
        Physics.Raycast(chainPos + Vector3.up * 100f, Vector3.down, out hit, 200f, SPLayers.InvertMaskPlayers, QueryTriggerInteraction.Ignore);
        return hit.point;
    }

    void OnDrawGizmos()
    {
        if (moveDest != Vector3.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(playerTransform.position + Vector3.up * .25f, playerTransform.position + Vector3.up * .25f + (moveDest - playerTransform.position).normalized);

        }
    }

}