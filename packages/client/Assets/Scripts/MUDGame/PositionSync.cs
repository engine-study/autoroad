using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{
    public Action OnMoveComplete;
    public PositionComponent Pos {get{return pos;}}

    [Header("Optional")]
    public Transform targetTransform;
    // [SerializeField] protected float speed = 1f;
    
    [Header("Debug")]
    [SerializeField] protected PositionComponent pos;
    [SerializeField] protected Vector3 targetPos;
    [SerializeField] bool moving = false;    
    public override System.Type TargetComponentType() {return typeof(PositionComponent);}

    protected override void Start() {

        base.Start();

        if(targetTransform == null) {
            targetTransform = transform;
        }

    }

    protected override void InitComponents() {
        base.InitComponents();

        pos = SyncComponent as PositionComponent;
    }

    protected override void InitialSync() {
        base.InitialSync();

        //set up our side of the compnents BEFORE 
        targetTransform.position = pos.Pos;
        targetPos = pos.Pos;
    }

    protected override void UpdateSync() {
        base.UpdateSync();

        if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateSource == UpdateSource.Revert)
        {
            targetTransform.position = pos.Pos;
            targetPos = pos.Pos;

            if(transform.position != targetPos) {
                EndMove();
            }
        }
        else if (syncType == ComponentSyncType.Lerp)
        {
            targetPos = pos.Pos;

            if(transform.position != targetPos) {
                StartMove();
            }
        }

    }

    protected override void UpdateLerp() {
        
        targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, ControllerMUD.MOVE_SPEED * Time.deltaTime);
        
        //turn off for efficiency until next update
        if(targetTransform.position == targetPos) {
            EndMove();
        }
    }

    void StartMove() {
        enabled = true;
        moving = true;
    }

    void EndMove() {
        enabled = false;
        moving = false;

        OnMoveComplete?.Invoke();
    }
}
