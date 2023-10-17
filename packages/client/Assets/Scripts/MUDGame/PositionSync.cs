using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class PositionSync : ComponentSync
{
    public Action OnMoveStart, OnMoveEnd;
    public Action OnMoveUpdate;
    public PositionComponent Pos {get{return pos;}}
    // public AnimationComponent Anim {get{return anim;}}
    public Vector3 GridPos {get{return gridPos;}}
    public Transform Target {get{return target;}}
    public float MoveLerp {get{return moveLerp;}}
    public bool Moving {get{return moving;}}

    [Header("Optional")]
    [SerializeField] Transform target;
    [SerializeField] public bool hideIfNoPosition = true;    
    [SerializeField] public bool hideIfBelowGround = true;    
    [SerializeField] public bool hideAfterLoaded = true;    
    [SerializeField] public bool rotateToFace = false;
    [SerializeField] float speed = 1f;
    [SerializeField] float rotationSpeed = 720f;
    
    [Header("Line")]
    [SerializeField] private bool useLine;
    [SerializeField] private bool parentTransformToEntity;
    private LineRenderer line;
    SPLerpCurve lerp;

    
    [Header("Debug")]
    [SerializeField] PositionComponent pos;
    [SerializeField] MoverMUD movement;
    [SerializeField] Vector3 gridPos;
    [SerializeField] Vector3 StartPos;
    [SerializeField] Vector3 TargetPos => pos.Pos; 
    [SerializeField] bool moving = false;    
    [SerializeField] float moveLerp = 0f;
    [SerializeField] float distanceMoved = 0f;
    [SerializeField] float distance = 0f;

    public override Type MUDComponentType() {return typeof(PositionComponent);}

    protected override void InitComponents() {
        base.InitComponents();

        if(target == null) { target = transform; }
        pos = SyncComponent as PositionComponent;
                
        if(parentTransformToEntity) {
            target.parent = pos.Entity.transform;
        }

    }


    protected override void InitialSync() {
        base.InitialSync();

        //set up our side of the compnents BEFORE 
        target.position = pos.Pos;
        StartPos = pos.Pos;
        UpdateGrid(pos.Pos);

        if(useLine) {
            line = ((GameObject)(Instantiate(Resources.Load("Prefabs/LinePosition"), transform))).GetComponent<LineRenderer>();
            line.enabled = false;
        }

        ourComponent.Toggle(IsVisible(), false);

    }

    public void SetMovement(MoverMUD newMove) {
        movement = newMove;
    }

    public bool IsVisible() {
        //hide us if we don't have a position
        return !(pos.UpdateInfo.UpdateType == UpdateType.DeleteRecord && hideIfNoPosition) && !(pos.PosLayer.y < 0 && hideIfBelowGround);
    }



    protected override void HandleUpdate() {
        base.HandleUpdate();

        if (syncType == ComponentSyncType.Lerp) {
            // if(target.position != TargetPos) StartMove();
            StartMove();
        } else if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateInfo.Source == UpdateSource.Revert) {
            EndMove();
        }

    }

    void UpdateGrid(Vector3 pos) {
        gridPos = new Vector3(Mathf.Round(pos.x),Mathf.Round(pos.y),Mathf.Round(pos.z));
    }

    void StartMove() {

        bool wasMoving = moving;

        enabled = true;
        moving = true;

        moveLerp = 0f;
        distanceMoved = 0f;
        StartPos = target.position;

        UpdateGrid(StartPos);

        distance = Vector3.Distance(StartPos, TargetPos);

        Debug.Log(gameObject.name + " MOVE: Start (" + (movement ? movement.name : "/") + ")", this);

        // if(action && !wasMoving) {action.PlayAnimation(true);}
        if(line) line.enabled = useLine;

        OnMoveStart?.Invoke();
    }

    void StartModifiedMove() {
        distance = Vector3.Distance(StartPos, TargetPos);
    }
    
    void EndMove() {

        bool wasMoving = moving;

        enabled = false;
        moving = false;
        target.position = pos.Pos;

        UpdateGrid(pos.Pos);

        Debug.Log(gameObject.name + " MOVE: End (" + (movement ? movement.name : "/") + ")", this);

        // if(action && wasMoving) {action.PlayAnimation(false);}
        if(line) line.enabled = false;
        if(hideAfterLoaded) { ourComponent.Toggle(IsVisible());}

        OnMoveEnd?.Invoke();
    }

    protected override void UpdateLerp() {
        base.UpdateLerp();

        UpdateRotation();
        UpdateMovement();

        OnMoveUpdate?.Invoke();

        UpdateState();

    }

    void UpdateRotation() {

        if(rotateToFace) {
            Quaternion lookRotation = target.rotation;

            var lookAt = TargetPos;
            lookAt.y = target.position.y;

            if (lookAt != target.position) {
                lookRotation = Quaternion.LookRotation(lookAt - target.position);
            }

            //ROTATE
            target.rotation = Quaternion.RotateTowards(target.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
    

    void UpdateMovement() {

        if(movement) {

            moveLerp = Mathf.Clamp01(distanceMoved / distance);
            target.position = movement.Move(StartPos, TargetPos, moveLerp);
            distanceMoved = distanceMoved + movement.Speed * Time.deltaTime;

        } else {        

            moveLerp = Mathf.Clamp01(distanceMoved / distance);
            target.position = Vector3.MoveTowards(target.position, TargetPos, speed * Time.deltaTime);
            distanceMoved = distanceMoved + speed * Time.deltaTime;

        }

        UpdateGrid(target.position);
     
    }

    void UpdateState() {
        if(useLine) {
            Vector3[] positions = new Vector3[] { target.position + Vector3.up * .05f, TargetPos + Vector3.up * .05f, TargetPos };
            line.SetPositions(positions);
        }

        //turn off for efficiency until next update
        if(target.position == TargetPos || moveLerp >= 1f) {
            EndMove();
        }
    }

    void OnDrawGizmos() {

        if(Application.isPlaying && Pos) {

            Gizmos.color = Moving ? Color.yellow : Color.green - Color.black * .5f;
            Gizmos.DrawWireCube(GridPos, Vector3.one * .9f - Vector3.up * .88f);

            if(Moving) {
                Gizmos.DrawLine(transform.position + Vector3.up * .1f, Pos.Pos + Vector3.up * .1f);
                Gizmos.DrawWireSphere(Pos.Pos + Vector3.up * .1f, .1f);
            }
        }
        
    }

}
