using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{

    [Header("Optional")]
    public Transform targetTransform;
    public float deltaSpeed = 2.5f;

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

    protected override void UpdateSync() {
        base.UpdateSync();

        if (syncType == ComponentSyncType.Lerp)
        {
            targetPos = pos.Pos;
            enabled = transform.position != targetPos;
        }
        else if (syncType == ComponentSyncType.Instant)
        {
            targetTransform.position = pos.Pos;
        }


    }

    protected override void UpdateLerp() {
        
        transform.position = Vector3.MoveTowards(transform.position, targetPos, deltaSpeed * Time.deltaTime);
        
        //turn off for efficiency until next update
        if(transform.position == targetPos) {
            enabled = false;
        }
    }
}
