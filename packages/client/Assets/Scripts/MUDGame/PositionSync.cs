using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{

    [Header("Optional")]
    public Transform targetTransform;
    // [SerializeField] protected float speed = 1f;
    
    [Header("Debug")]
    protected PositionComponent pos;
    protected Vector3 targetPos;

    public override System.Type TargetComponentType() {return typeof(PositionComponent);}

    protected override void Start() {

        base.Start();

        if(targetTransform == null) {
            targetTransform = transform;
        }

    }

    protected override void InitComponents() {
        base.InitComponents();

        pos = targetComponent as PositionComponent;

    }

    protected override void InitialSync() {
        base.InitialSync();

        //set up our side of the compnents BEFORE 
        targetTransform.position = pos.Pos;
        targetPos = pos.Pos;
    }

    protected override void UpdateSync(UpdateEvent updateType) {
        base.UpdateSync(updateType);

        if (syncType == ComponentSyncType.Instant || updateType == UpdateEvent.Revert)
        {
            targetTransform.position = pos.Pos;
            targetPos = pos.Pos;
            enabled = false;
        }
        else if (syncType == ComponentSyncType.Lerp)
        {
            targetPos = pos.Pos;
            enabled = transform.position != targetPos;
        }


    }

    protected override void UpdateLerp() {
        
        targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, ControllerMUD.MOVE_SPEED * Time.deltaTime);
        
        //turn off for efficiency until next update
        if(targetTransform.position == targetPos) {
            enabled = false;
        }
    }
}
