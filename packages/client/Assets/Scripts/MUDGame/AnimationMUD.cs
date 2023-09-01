using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class AnimationMUD : MonoBehaviour
{

    public ActionName Action {get{return action;}}
    public ActionComponent ActionComponent {get{return actionComponent;}}
    public PositionSync PositionSync { get; private set; }
    public SPAnimator Animator { get; private set; }

    [Header("Animation")]
    [SerializeField] Transform target;
    [SerializeField] ActionData actionData;
    
    [Header("Debug")]
    [SerializeField] ActionName action;
    [SerializeField] ActionEffect actionEffect;
    [SerializeField] MUDEntity entity;
    [SerializeField] ActionComponent actionComponent;

    protected virtual void Awake() {
      
    }

    protected virtual void Start() {
        
        Animator = GetComponentInChildren<SPAnimator>();
        entity = GetComponentInParent<MUDEntity>();
        PositionSync = GetComponentInParent<PositionSync>();
        actionData = Instantiate(actionData, transform.position, transform.rotation, transform);
        if(target == null) target = transform;
        
        if(entity.HasInit) Init();
        else entity.OnInit += Init;
    }

    void Init() {
        actionComponent = entity.GetMUDComponent<ActionComponent>();
        if (actionComponent == null) { Debug.LogError("No action component"); return; }

        actionComponent.OnRichUpdate += UpdateAction;
    }

    void OnDestroy() {

        if(entity) entity.OnInit -= Init;
        if(actionComponent) actionComponent.OnRichUpdate -= UpdateAction;
        
    }

    void UpdateAction() {
        EnterState(actionComponent.Action);
    }


    public virtual void EnterState(ActionName newAction) {

        //turn off old action
        if (actionEffect != null && newAction != action) { ToggleAction(false, actionEffect); }

        Debug.Log("Action: " + newAction.ToString());

        action = newAction;
        actionEffect = actionData.effects[(int)action];

        //play new action
        if(actionEffect) { ToggleAction(true, actionEffect);}

    }   

    public virtual void ToggleAction(bool toggle, ActionEffect newAction) {
        actionEffect.Toggle(toggle, this);
    }
    
}
