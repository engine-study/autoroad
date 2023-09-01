using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class AnimationMUD : MonoBehaviour
{

    public ActionName Action {get{return action;}}
    public ActionComponent ActionComponent {get{return actionComponent;}}

    [Header("Animation")]
    [SerializeField] Transform target;
    [SerializeField] ActionData actionData;
    SPAnimator animator;
    PositionComponent pos;
    
    [Header("Debug")]
    [SerializeField] ActionName action;
    [SerializeField] ActionEffect actionEffect;
    [SerializeField] MUDEntity entity;
    [SerializeField] ActionComponent actionComponent;
    [SerializeField] PositionSync positionSync;
    int actionIndex;

    protected virtual void Awake() {
        animator = GetComponentInChildren<SPAnimator>();
        entity = GetComponentInParent<MUDEntity>();
        positionSync = GetComponentInParent<PositionSync>();
        actionData = Instantiate(actionData, transform.position, transform.rotation, transform);
        if(target == null) target = transform;
        
        if(entity.HasInit) Init();
        else entity.OnInit += Init;
    }

    void Init() {
        actionComponent = entity.GetMUDComponent<ActionComponent>();
        if (actionComponent == null) { Debug.LogError("No action component"); return; }

        actionComponent.OnUpdated += UpdateAction;
    }

    void OnDestroy() {

        if(entity) entity.OnInit -= Init;
        if(actionComponent) actionComponent.OnUpdated -= UpdateAction;
        
    }

    void UpdateAction() {
        EnterState(actionComponent.Action);
    }


    public virtual void EnterState(ActionName newAction) {

        if (actionEffect != null && newAction != action) { ToggleAction(false, actionEffect); }

        Debug.Log("Action: " + newAction.ToString());

        action = newAction;
        actionIndex = (int)actionComponent.Action;
        actionEffect = actionData.effects[actionIndex];

        if(actionEffect) {
            ToggleAction(true, actionEffect);
        }

        if(actionData.effects[actionIndex] != null) {

        }
    }   

    public virtual void ToggleAction(bool toggle, ActionEffect newAction) {

        if(newAction.gameObject.activeInHierarchy) {
            if(newAction.effect) {
                if(toggle) { newAction.effect.PlayEnabled();
                } else {newAction.effect.PlayDisabled();}
            }

        }

        if(positionSync) positionSync.SetMovement(newAction.movement);

        newAction.gameObject.SetActive(toggle);

        if(toggle) {

            if(animator) {
                if (!string.IsNullOrEmpty(newAction.animationClip)) {
                    animator.PlayClip(newAction.animationClip);
                }
                
                if(newAction.action) {
                    if(newAction.action is SPActionPlayer) {
                        SPActionPlayer action = newAction.action as SPActionPlayer;
                        action.animatorState?.Apply(animator);
                    }
                }
            }

        } else {

        }

    }


    // Vector3 lookVector;
    // Quaternion lookRotation;
    // public void SetLookRotation(Vector3 newLookAt) {
    //    var _lookY = newLookAt;
    //     _lookY.y = target.position.y;

    //     if (_lookY != target.position) {
    //         Vector3 eulerAngles = Quaternion.LookRotation(_lookY - target.position).eulerAngles;
    //         lookVector = (_lookY - target.position).normalized;
    //         lookRotation = Quaternion.Euler(eulerAngles.x, (int)Mathf.Round(eulerAngles.y / 90) * 90, eulerAngles.z);
    //     }
    // }

    

}
