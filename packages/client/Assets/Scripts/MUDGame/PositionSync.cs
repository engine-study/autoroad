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
    public Transform Target {get{return target;}}
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
    [SerializeField] AnimationComponent anim;
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

    public bool IsVisible() {
        //hide us if we don't have a position
        return !(pos.UpdateInfo.UpdateType == UpdateType.DeleteRecord && hideIfNoPosition) && !(pos.PosLayer.y < 0 && hideIfBelowGround);
    }



    protected override void UpdateSync() {
        base.UpdateSync();

        if (anim == null) { 
            anim = pos.Entity.GetMUDComponent<AnimationComponent>();
            if (anim) { anim.transform.parent = target; anim.transform.localPosition = Vector3.zero; anim.transform.localRotation = Quaternion.identity; }
        }

        Vector3 newPos = pos.Pos;

        if (syncType == ComponentSyncType.Lerp) {
            if(target.position != newPos && targetPos != newPos) {StartMove();}
        } else if (syncType == ComponentSyncType.Instant || SyncComponent.UpdateInfo.Source == UpdateSource.Revert) {
            target.position = pos.Pos;
            if(moving) {
                EndMove();
            }
        }

        if(hideAfterLoaded && gameObject.activeInHierarchy != IsVisible()) {
            gameObject.SetActive(IsVisible());
        }

    }

    void StartMove() {

        enabled = true;
        moving = true;

        moveLerp = 0f;
        startPos = target.position;
        targetPos = pos.Pos;

        Debug.Log("PositionSync: Start");

        if(anim) {anim.PlayAnimation(true);}
        if(line) line.enabled = useLine;
    }
    
    void EndMove() {
        enabled = false;
        moving = false;
        targetPos = pos.Pos;
        target.position = pos.Pos;

        Debug.Log("PositionSync: End");

        if(anim) {anim.PlayAnimation(false);}
        if(line) line.enabled = false;

        OnMoveComplete?.Invoke();
    }

    protected override void UpdateLerp() {

        UpdateRotation();
        UpdateMovement();
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

        if(anim) {
            if(anim.Anim == AnimationType.Walk) {
                target.position = Vector3.MoveTowards(target.position, targetPos, speed * Time.deltaTime);
            } else if (anim.Anim == AnimationType.Hop) {
                moveLerp += Time.deltaTime * 1.5f;
                Vector3 vertical = Vector3.up * 2f * Mathf.Sin(Mathf.PI * moveLerp);
                target.position = Vector3.LerpUnclamped(startPos + vertical, targetPos + vertical, anim.Evaluate(moveLerp));
            } else if (anim.Anim == AnimationType.Teleport) {
                target.position = targetPos;
            }
        } else {
            target.position = Vector3.MoveTowards(target.position, targetPos, speed * Time.deltaTime);
        }
     
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
