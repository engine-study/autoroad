#nullable enable
using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using IWorld.ContractDefinition;
using mud.Unity;
using UniRx;
using UnityEngine;
using ObservableExtensions = UniRx.ObservableExtensions;

public class ControllerMUD : SPController
{
    private Vector3? _onchainPosition;
    private Vector3 enginePosition;

	Quaternion lookRotation;

    public GameObject moveMarker;
    Vector3 markerPos;

    private IDisposable? _disposer;
    private PlayerSync _player;

    float alive = 0f;
    float distance;
    public AudioClip [] footsteps;
    public AudioClip [] pushes;
    public AudioClip [] pops;
    

	void Awake() {
		
        moveMarker.transform.parent = null;
        moveMarker.transform.position = transform.position;

        // moveMarker.SetActive(false);
        _player = GetComponent<PlayerSync>();
        _disposer = ObservableExtensions.Subscribe(PositionTable.OnRecordInsert().Merge(PositionTable.OnRecordUpdate()).ObserveOnMainThread(),
                OnChainPositionUpdate);

        
	}

    public override void Init()
    {
        base.Init();

        controller.enabled = false;

    }

    public override void ToggleController(bool toggle)
    {
        base.ToggleController(toggle);

        controller.enabled = false;
        enabled = true;
    }

    private void OnChainPositionUpdate(PositionTableUpdate update)
    {
        if (_player.key == null || update.Key != _player.key) return;
        
        // if (_player.IsLocalPlayer()) {
        //     moveMarker.SetActive(false);
        // }


        var currentValue = update.TypedValue.Item1;
        if (currentValue == null) return;
        var x = Convert.ToSingle(currentValue.x);
        var y = Convert.ToSingle(currentValue.y);

        Vector3 lastOnchainPos = _onchainPosition == null ? transform.position : (Vector3)_onchainPosition;
    
        _onchainPosition = new Vector3(x, 0, y);
        markerPos = (Vector3)_onchainPosition;

        Debug.Log("New Pos: " + SPHelper.GiveTruncatedHash(update.Key));
        Debug.Log("New Pos: " + _onchainPosition.ToString());
		
        UpdateAnimation((Vector3)_onchainPosition);

        // transform.position = (Vector3)_destination;
    }

    
    private async UniTaskVoid SendMoveFromTx(int x, int y)
    {
        try
        {
            // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
            await NetworkManager.Instance.worldSend.TxExecute<MoveFromFunction>(x, y);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async UniTaskVoid SendPushTx(int x, int y, int pushX, int pushY)
    {
        try
        {
            // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
            await NetworkManager.Instance.worldSend.TxExecute<PushFunction>(x, y,pushX,pushY);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
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

    void Update() {

        alive += Time.deltaTime;

        UpdateInput();
        UpdatePosition();

    }

    Vector3 moveDest;
    void UpdateInput()
    {
		if(!hasInit) {
			return;
		}

		if (moveTimeout > 0f)
        {
            moveTimeout -= Time.deltaTime;
            return;
        }

        if (!_player.IsLocalPlayer())
            return;

        bool push = Input.GetKey(KeyCode.LeftShift);

        bool input = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (!input)
            return;

        moveDest = Vector3.zero;

        if(_onchainPosition == null) {
            moveDest = transform.position;
            moveDest.y = 0f;
        } else {
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


        if(push) {

            Debug.Log("PUSHING");

            Vector3 direction = (moveDest-transform.position).normalized;
            moveDest = new Vector3(Mathf.Round(transform.position.x + direction.x), 0f, Mathf.Round(transform.position.z + direction.z));
            Vector3 pushToPos = new Vector3(Mathf.Round(moveDest.x + direction.x), 0f, Mathf.Round(moveDest.z + direction.z));

            SendPushTx(Convert.ToInt32(moveDest.x), Convert.ToInt32(moveDest.z), Convert.ToInt32(pushToPos.x), Convert.ToInt32(pushToPos.z)).Forget();
        } else {

            Debug.Log("WALKING");
            markerPos = moveDest;
            SendMoveFromTx(Convert.ToInt32(moveDest.x), Convert.ToInt32(moveDest.z)).Forget();
        }

        markerPos = moveDest;
        moveTimeout = .75f;

        UpdateAnimation(moveDest);

        // UIMother.PlayAccept();

    }

    
    public void UpdatePosition() {

        if (_onchainPosition != null && transform.position != _onchainPosition)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _onchainPosition.Value, 2.5f * Time.deltaTime);
            distance += Vector3.Distance(transform.position,newPosition);

            transform.position = newPosition;
	
	    }

        // Determine the new rotation
        var newRotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 720f * Time.deltaTime);
        transform.rotation = newRotation;

        moveMarker.transform.position = Vector3.Lerp(moveMarker.transform.position, markerPos + Vector3.up * .1f + Vector3.up * Mathf.Sin(Time.time * 5f) * .1f,.2f);

        if(alive > 1f && distance > 1f) {
            distance -= 1f;
            player.resources.sfx.PlaySound(footsteps);
        }

    }

    void UpdateAnimation(Vector3 toPosition) {


        RaycastHit hit;
        Vector3 direction = (toPosition-transform.position).normalized;
		Physics.Raycast(transform.position + Vector3.up * .25f, direction, out hit, 1f, Physics.DefaultRaycastLayers ,QueryTriggerInteraction.Ignore);
        Debug.Log("Hit: " + (hit.collider?hit.collider.gameObject.name:"No"));

        if(transform.position == toPosition) {
            
            player?.Animator.PlayClip("Tired");

        } else if(hit.collider && (hit.collider.GetComponentInParent<Rock>() != null || hit.collider.GetComponentInParent<Player>() != null)) {

            // Debug.Log("PUSHING");

            Vector3 hitGrid = new Vector3(Mathf.Round(hit.point.x), 0f, Mathf.Round(hit.point.z));
            Vector3 pushSpot = new Vector3(Mathf.Round(hitGrid.x + direction.x), 0f, Mathf.Round(hitGrid.z + direction.z));

            player?.Animator.PlayClip("Push");
            markerPos = hitGrid;


        } else {

            // Debug.Log("WALKING");
            player?.Animator.PlayClip("Idle");
            markerPos = toPosition;

        }

        toPosition.y = ChainPosToEngine(toPosition).y;
		enginePosition = toPosition;

		var _lookY = toPosition;
		_lookY.y = transform.position.y;

        if(_lookY != transform.position) {
		    lookRotation = Quaternion.LookRotation(_lookY - transform.position);
        }
    }


    Vector3 ChainPosToEngine(Vector3 chainPos) {
        RaycastHit hit;
		Physics.Raycast(chainPos + Vector3.up * 100f, Vector3.down, out hit, 200f, SPLayers.InvertMaskPlayers,QueryTriggerInteraction.Ignore);
        return hit.point;
    }

    void OnDrawGizmos() {
        if(moveDest != Vector3.zero) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * .25f, transform.position + Vector3.up * .25f + (moveDest-transform.position).normalized);
            
        }
    }

    private void OnDestroy()
    {
        _disposer?.Dispose();
    }
}