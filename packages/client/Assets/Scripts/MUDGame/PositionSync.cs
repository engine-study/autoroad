using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{

    [Header("Optional")]
    public Transform targetTransform;
    public float lerpSpeed = .1f;

    [Header("Debug")]
    protected Position pos;
    protected Vector3 targetPos;

    protected override string GetComponentName() {return "Position";}

    protected override void Awake() {

        if(targetTransform == null) {
            targetTransform = transform;
        }

        base.Awake();
    }

    protected override void InitComponents() {
        base.InitComponents();

        pos = targetComponent as Position;

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
        }
        else if (syncType == ComponentSyncType.Instant)
        {
            targetTransform.position = pos.Pos;
        }


    }

    protected override void UpdateLerp() {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed);
    }
}
