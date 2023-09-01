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
    Dictionary<string, ActionEffect> effects = new Dictionary<string, ActionEffect>();

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

        if(target == null) target = transform;
        
        if(entity.HasInit) Init();
        else entity.OnInit += Init;
    }

    void Init() {
        actionComponent = entity.GetMUDComponent<ActionComponent>();
        if (actionComponent == null) { Debug.LogError("No action component", this); return; }

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

        Debug.Log("Action: " + newAction.ToString(), this);

        action = newAction;
        actionEffect = LoadAction(action.ToString());

        //play new action
        if(actionEffect) { ToggleAction(true, actionEffect);}

    }   

    ActionEffect LoadAction(string action) {

        effects.TryGetValue(action, out ActionEffect newAction);
        
        if(newAction == null) {

            object resource = Resources.Load("Action/" + action);
            if(resource == null) return null;

            newAction = Instantiate((resource as GameObject), transform.position, transform.rotation, transform).GetComponent<ActionEffect>();
            effects.Add(action, newAction);
        }

        return newAction;
    }

    

    public virtual void ToggleAction(bool toggle, ActionEffect newAction) {
        actionEffect.Toggle(toggle, this);
    }
    
}
