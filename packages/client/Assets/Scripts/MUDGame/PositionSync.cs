using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{
    public Action OnMoveComplete;
    public PositionComponent Pos {get{return pos;}}
    public bool Moving {get{return moving;}}

    [Header("Optional")]
    [SerializeField] Transform targetTransform;
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
    
    
    [Header("Debug")]
    [SerializeField] PositionComponent pos;
    [SerializeField] Vector3 targetPos;
    [SerializeField] bool moving = false;    
    public override MUDComponent SyncedComponent() {return new PositionComponent();}

    protected override void Awake() {
        base.Awake();

        if(targetTransform == null) {
            targetTransform = transform;
        }
    }

    protected override void InitComponents() {
        base.InitComponents();

        pos = SyncComponent as PositionComponent;

        if(parentTransformToEntity) {
            targetTransform.parent = pos.Entity.transform;
        }

    }


    protected override void InitialSync() {
        base.InitialSync();

        //set up our side of the compnents BEFORE 
        targetTransform.position = pos.Pos;
        targetPos = pos.Pos;

        if(useLine) {
            line = ((GameObject)(Instantiate(Resources.Load("Prefabs/LinePosition"), transform))).GetComponent<LineRenderer>();
        }

        gameObject.SetActive(IsVisible());

    }

    public bool IsVisible() {
        //hide us if we don't have a position
        return !(pos.UpdateInfo.UpdateType == UpdateType.DeleteRecord && hideIfNoPosition) && !(pos.PosLayer.y < 0 && hideIfBelowGround);
    }



    protected override void UpdateSync() {
        base.UpdateSync();

        if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateInfo.Source == UpdateSource.Revert) {
            targetTransform.position = pos.Pos;
            targetPos = pos.Pos;

            if(transform.position != targetPos) {
                EndMove();
            }
        } else if (syncType == ComponentSyncType.Lerp) {
            targetPos = pos.Pos;

            if(transform.position != targetPos) {
                StartMove();
            }
        }

        if(hideAfterLoaded && gameObject.activeInHierarchy != IsVisible()) {
            gameObject.SetActive(IsVisible());
        }

    }

    protected override void UpdateLerp() {
        
        if(rotateToFace) {

            Quaternion lookRotation = targetTransform.rotation;

            var lookAt = targetPos;
            lookAt.y = targetTransform.position.y;

            if (lookAt != targetTransform.position) {
                lookRotation = Quaternion.LookRotation(lookAt - targetTransform.position);
            }

            //ROTATE
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        
        targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, speed * Time.deltaTime);

        if(useLine) {
            Vector3[] positions = new Vector3[] { targetTransform.position + Vector3.up * .05f, targetPos + Vector3.up * .05f, targetPos };
            line.SetPositions(positions);
        }


        //turn off for efficiency until next update
        if(targetTransform.position == targetPos) {
            EndMove();
        }
    }

    void StartMove() {
        enabled = true;
        moving = true;

        if(line) line.enabled = useLine;

    }

    void EndMove() {
        enabled = false;
        moving = false;

        if(line) line.enabled = false;

        OnMoveComplete?.Invoke();
    }
}
