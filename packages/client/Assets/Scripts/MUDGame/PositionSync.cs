using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class PositionSync : ComponentSync
{
    public Action OnMoveComplete;
    public PositionComponent Pos {get{return pos;}}
    public AnimationComponent Anim {get{return anim;}}
    public Transform Target {get{return targetTransform;}}
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
    SPLerpCurve lerp;

    
    [Header("Debug")]
    [SerializeField] PositionComponent pos;
    [SerializeField] AnimationComponent anim;
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

        if (anim == null) { 
            anim = pos.Entity.GetMUDComponent<AnimationComponent>();
            if (anim) { anim.transform.parent = targetTransform; anim.transform.localPosition = Vector3.zero; anim.transform.localRotation = Quaternion.identity; }
        }

        Vector3 newPos = pos.Pos;
        targetPos = pos.Pos;

        if (syncType == ComponentSyncType.Lerp) {
            if(targetTransform.position != targetPos) {StartMove();}
        } else if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateInfo.Source == UpdateSource.Revert) {
            targetTransform.position = pos.Pos;
            EndMove();
        }

        if(hideAfterLoaded && gameObject.activeInHierarchy != IsVisible()) {
            gameObject.SetActive(IsVisible());
        }

    }


    void StartMove() {

        enabled = true;
        moving = true;

        Debug.Log("PositionSync: Start");

        if(anim) {anim.PlayAnimation();}
        if(line) line.enabled = useLine;
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
            targetTransform.rotation = Quaternion.RotateTowards(targetTransform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        
        if(anim) {
            UpdateAnim();
        } else {
            targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, speed * Time.deltaTime);
        }

        if(useLine) {
            Vector3[] positions = new Vector3[] { targetTransform.position + Vector3.up * .05f, targetPos + Vector3.up * .05f, targetPos };
            line.SetPositions(positions);
        }


        //turn off for efficiency until next update
        if(targetTransform.position == targetPos) {
            EndMove();
        }
    }

    void UpdateAnim() {
        if(anim.Anim == AnimationType.Walk) {
            targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, speed * Time.deltaTime);
        } else if (anim.Anim == AnimationType.Hop) {
            targetTransform.position = Vector3.MoveTowards(targetTransform.position, targetPos, speed * Time.deltaTime);
        } else if (anim.Anim == AnimationType.Teleport) {
            targetTransform.position = targetPos;
            if(anim) {anim.PlayAnimation();}
        }
    }



    void EndMove() {
        enabled = false;
        moving = false;

        Debug.Log("PositionSync: End");

        if(line) line.enabled = false;

        OnMoveComplete?.Invoke();
    }
}
