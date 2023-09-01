using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{
    public Action OnMoveStart, OnMoveComplete;
    public Action OnMoveUpdate;
    public PositionComponent Pos {get{return pos;}}
    // public AnimationComponent Anim {get{return anim;}}
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
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 targetPos;
    [SerializeField] bool moving = false;    
    [SerializeField] float moveLerp = 0f;

    public override MUDComponent SyncedComponent() {return new PositionComponent();}

    protected override void Awake() {
        base.Awake();

        if(target == null) {
            target = transform;
        }
    }

    protected override void InitComponents() {
        base.InitComponents();

        pos = SyncComponent as PositionComponent;
                
        if(parentTransformToEntity) {
            target.parent = pos.Entity.transform;
        }

    }


    protected override void InitialSync() {
        base.InitialSync();

        //set up our side of the compnents BEFORE 
        target.position = pos.Pos;
        startPos = pos.Pos;
        targetPos = pos.Pos;

        if(useLine) {
            line = ((GameObject)(Instantiate(Resources.Load("Prefabs/LinePosition"), transform))).GetComponent<LineRenderer>();
        }

        gameObject.SetActive(IsVisible());

    }

    public void SetMovement(MoverMUD newMove) {
        movement = newMove;
    }

    public bool IsVisible() {
        //hide us if we don't have a position
        return !(pos.UpdateInfo.UpdateType == UpdateType.DeleteRecord && hideIfNoPosition) && !(pos.PosLayer.y < 0 && hideIfBelowGround);
    }



    protected override void UpdateSync() {
        base.UpdateSync();

        Vector3 newPos = pos.Pos;

        if (syncType == ComponentSyncType.Lerp) {
            if(target.position != newPos && targetPos != newPos) {StartMove();}
        } else if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateInfo.Source == UpdateSource.Revert) {
            target.position = pos.Pos;
            if(moving) {
                EndMove();
            }
        }

    }

    void StartMove() {

        bool wasMoving = moving;

        enabled = true;
        moving = true;

        moveLerp = 0f;
        startPos = target.position;
        targetPos = pos.Pos;

        Debug.Log("PositionSync: Start");

        // if(action && !wasMoving) {action.PlayAnimation(true);}
        if(line) line.enabled = useLine;

        OnMoveStart?.Invoke();
    }
    
    void EndMove() {

        bool wasMoving = moving;

        enabled = false;
        moving = false;
        targetPos = pos.Pos;
        target.position = pos.Pos;

        Debug.Log("PositionSync: End");

        // if(action && wasMoving) {action.PlayAnimation(false);}
        if(line) line.enabled = false;

        if(hideAfterLoaded && gameObject.activeInHierarchy != IsVisible()) {
            gameObject.SetActive(IsVisible());
        }

        OnMoveComplete?.Invoke();
    }

    protected override void UpdateLerp() {

        UpdateRotation();
        UpdateMovement();

        OnMoveUpdate?.Invoke();

        UpdateState();

    }

    void UpdateRotation() {

        if(rotateToFace) {
            Quaternion lookRotation = target.rotation;

            var lookAt = targetPos;
            lookAt.y = target.position.y;

            if (lookAt != target.position) {
                lookRotation = Quaternion.LookRotation(lookAt - target.position);
            }

            //ROTATE
            target.rotation = Quaternion.RotateTowards(target.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
    

    void UpdateMovement() {

        moveLerp = Mathf.Clamp01(moveLerp + Time.deltaTime);

        if(movement == null) {
            target.position = Vector3.MoveTowards(target.position, targetPos, speed * Time.deltaTime);
            return;
        }

        target.position = movement.Move(startPos, targetPos, moveLerp);
     
    }

    void UpdateState() {
        if(useLine) {
            Vector3[] positions = new Vector3[] { target.position + Vector3.up * .05f, targetPos + Vector3.up * .05f, targetPos };
            line.SetPositions(positions);
        }

        //turn off for efficiency until next update
        if(target.position == targetPos || moveLerp >= 1f) {
            EndMove();
        }
    }

}
