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
    public SPController Controller { get; private set; }
    public SPLooker Look {get{return looker;}}

    [Header("Animation")]
    [SerializeField] Transform target;
    Transform head;
    Rigidbody headRB;
    Transform headParent;
    Vector3 headPosLocal;
    Quaternion headRotLocal;
    Dictionary<string, ActionEffect> effects = new Dictionary<string, ActionEffect>();

    [Header("Debug")]
    [SerializeField] ActionName action;
    [SerializeField] ActionEffect actionEffect;
    [SerializeField] MUDEntity entity;
    [SerializeField] ActionComponent actionComponent;
    SPLooker looker;
    protected virtual void Awake() {
        if(target == null) target = transform;

        entity = GetComponentInParent<MUDEntity>();
        looker = target.gameObject.AddComponent<SPLooker>();

    }

    protected virtual void Start() {

        Animator = GetComponentInChildren<SPAnimator>(true);
        Controller = GetComponentInChildren<SPController>(true);
        if(Controller == null) {
            Controller = gameObject.AddComponent<SPController>();
            Controller.ToggleController(false);
        }

        PositionSync = GetComponentInParent<PositionSync>(true);

        head = Animator.Head;
        headRB = head.GetComponent<Rigidbody>();
        headParent = head.transform.parent;
        headPosLocal = head.localPosition;
        headRotLocal = head.localRotation;

        if(entity == null) return;
        
        actionComponent = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);

        if(entity.HasInit) Init();
        else entity.OnInit += Init;
    }

    void Init() {
        actionComponent = entity.GetMUDComponent<ActionComponent>();
        if (actionComponent == null) { Debug.LogError("No action component", this); return; }

        actionComponent.OnUpdated += UpdateAction;
    }

    void OnDestroy() {

        if(entity) entity.OnInit -= Init;
        if(actionComponent) actionComponent.OnUpdated -= UpdateAction;
        
    }

    void UpdateAction() {
        EnterState(actionComponent.Action);
    }


    bool isSimple;
    public void ToggleSimple(bool toggle) {
        isSimple = toggle;
        if(toggle) {
            head.parent = transform;
            head.localPosition = Vector3.up;
            head.localRotation = Quaternion.identity;
        } else {
            head.parent = headParent;
            head.localPosition = headPosLocal;
            head.localRotation = headRotLocal;
        }
    }

    public void ToggleRagdoll(bool toggle) {
        
        Controller.Ragdoll(toggle);

        if(toggle) {
            headRB.isKinematic = false;
            headRB.velocity = Random.onUnitSphere * .1f;
            head.parent = transform;
        } else {
            //set head back to where its supposed to be
            headRB.isKinematic = true;
            ToggleSimple(isSimple);
        }

    }

    public virtual void EnterState(ActionName newAction) {

        //turn off old action
        if (actionEffect != null && newAction != action) { 
            ToggleAction(false, actionEffect); 
        }

        Debug.Log(actionComponent.Entity.Name + " [ANIM]: [" + newAction.ToString().ToUpper() + "] (" + (int)actionComponent.Position.x + "," + (int)actionComponent.Position.z + ")", this);

        action = newAction;
        actionEffect = LoadAction(action.ToString());
        looker.SetLookRotation(actionComponent.Position);

        //play new action if it exists
        if(actionEffect == null) return;

        ToggleAction(true, actionEffect);
        // actionCoroutine = StartCoroutine(ActionCoroutine(actionEffect));
        
    }   

    public virtual void ToggleDead(bool toggle) {
        if(toggle) {
            
        } else {

        }
    }

    public virtual void ToggleAction(bool toggle, ActionEffect newAction) {
        actionEffect.Toggle(toggle, this);
    }

    
    //load from resources folder
    ActionEffect LoadAction(string action) {

        effects.TryGetValue(action, out ActionEffect newAction);
        
        if(newAction == null) {

            object resource = Resources.Load("Action/" + action);
            if(resource == null) return null;

            newAction = Instantiate((resource as GameObject), transform).GetComponent<ActionEffect>();
            newAction.transform.localPosition = Vector3.zero;
            newAction.transform.localRotation = Quaternion.identity;
            
            effects.Add(action, newAction);
        }

        return newAction;
    }
  

}
