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
    public SPLooker Look {get{return looker;}}

    [Header("Animation")]
    [SerializeField] Transform target;
    Dictionary<string, ActionEffect> effects = new Dictionary<string, ActionEffect>();

    [Header("Debug")]
    [SerializeField] ActionName action;
    [SerializeField] ActionEffect actionEffect;
    [SerializeField] MUDEntity entity;
    [SerializeField] ActionComponent actionComponent;
    SPLooker looker;
    protected virtual void Awake() {
        entity = GetComponentInParent<MUDEntity>();
    }

    protected virtual void Start() {

        actionComponent = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);
        Animator = GetComponentInChildren<SPAnimator>();
        PositionSync = GetComponentInParent<PositionSync>();

        if(target == null) target = transform;

        looker = target.gameObject.AddComponent<SPLooker>();

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
        if (actionEffect != null && newAction != action) { 
            if (actionCoroutine != null) { StopCoroutine(actionCoroutine); }
            ToggleAction(false, actionEffect); 
        }

        Debug.Log("ACTIONSET", this);
        Debug.Log(newAction.ToString(), this);
        Debug.Log(((int)actionComponent.Position.x).ToString(), this);
        Debug.Log(((int)actionComponent.Position.z).ToString(), this);

        action = newAction;
        actionEffect = LoadAction(action.ToString());
        looker.SetLookRotation(actionComponent.Position);

        //play new action if it exists
        if(actionEffect == null) return;

        ToggleAction(true, actionEffect);
        // actionCoroutine = StartCoroutine(ActionCoroutine(actionEffect));
        
    }   

    public virtual void ToggleAction(bool toggle, ActionEffect newAction) {
        actionEffect.Toggle(toggle, this);
    }

    Coroutine actionCoroutine;
    IEnumerator ActionCoroutine(ActionEffect newAction) {
        yield return null;
        ToggleAction(true, newAction);

        yield return new WaitForSeconds(2f);
        ToggleAction(false, newAction);

    }
    
    
    //load from resources folder
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
  

}
