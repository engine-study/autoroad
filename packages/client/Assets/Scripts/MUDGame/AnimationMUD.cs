using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class AnimationMUD : MonoBehaviour
{
    public ActionName Action {get{return actionEffect ? actionEffect.Action : ActionName.Idle;}}
    public ActionComponent ActionComponent {get{return actionComponent;}}
    public SPLooker Look {get{return looker;}}
    public IActor Actor {get{return actor;}}

    [Header("Animation")]
    [SerializeField] Transform root;
    Transform head;
    Rigidbody headRB;
    Transform headParent;
    Vector3 headPosLocal;
    Quaternion headRotLocal;
    Dictionary<string, ActionEffect> effects = new Dictionary<string, ActionEffect>();

    [Header("Action Debug")]
    [SerializeField] ActionEffect actionEffect;
    [SerializeField] MUDEntity target;
    [SerializeField] PositionSync targetPos;

    [Header("Linked")]
    public PositionSync PositionSync;
    public SPAnimator Animator;
    public SPController Controller;
    [SerializeField] MUDEntity entity;
    [SerializeField] ActionComponent actionComponent;
    [SerializeField] SPLooker looker;
    [SerializeField] IActor actor;

    protected virtual void Awake() {
        if(root == null) root = transform;

        entity = GetComponentInParent<MUDEntity>();
        looker = root.gameObject.AddComponent<SPLooker>();

    }

    protected virtual void Start() {

        Animator = GetComponentInChildren<SPAnimator>(true);
        Controller = GetComponentInChildren<SPController>(true);
        if(Controller == null) {
            Controller = gameObject.AddComponent<SPController>();
            Controller.ToggleController(false);
        }

        PositionSync = GetComponentInParent<PositionSync>(true);

        actor = new ActorAnimator(Animator);

        head = Animator.Head;
        headRB = head.GetComponent<Rigidbody>();
        headParent = head.transform.parent;
        headPosLocal = head.localPosition;
        headRotLocal = head.localRotation;

        if(entity == null) return;
        
        actionComponent = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);

        if(entity.Loaded) Init();
        else entity.OnLoaded += Init;
    }

    void Init() {
        actionComponent = entity.GetMUDComponent<ActionComponent>();
        if (actionComponent == null) { Debug.LogError("No action component", this); return; }

        actionComponent.OnUpdated += UpdateAction;
    }

    void OnDestroy() {

        if(entity) entity.OnLoaded -= Init;
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
            head.parent = transform;
            headRB.isKinematic = false;
            headRB.velocity = Random.onUnitSphere * .1f;
        } else {
            //set head back to where its supposed to be
            headRB.isKinematic = true;
            ToggleSimple(isSimple);
        }

    }

    public virtual void EnterState(ActionName newAction) {

        Debug.Log(actionComponent.Entity.Name + " [ANIM]: [" + newAction.ToString().ToUpper() + "] (" + (int)actionComponent.Position.x + "," + (int)actionComponent.Position.z + ")", this);
        ActionEffect newEffect = LoadAction(newAction.ToString());

        if(newEffect == null) {
            ToggleAction(false, actionEffect);
            actionEffect = null;
            return;
        } else {
            if(newEffect.Action != newAction) {Debug.LogError(newAction + " doesn't match on " + newEffect.name);}
        }

        if(WaitAFrame != null) StopCoroutine(WaitAFrame); 
        WaitAFrame = StartCoroutine(AnimationInsanity(newEffect));
        
    }   

    Coroutine WaitAFrame;
    IEnumerator AnimationInsanity(ActionEffect newAction) {

        //wait a frame so all the positions are synced
        yield return null;
        
        looker.SetLookRotation(actionComponent.Position);
        target = GridMUD.GetEntityAt(actionComponent.Position);
        targetPos = target && target != actionComponent.Entity ? target.GetRootComponent<PositionSync>() : null;

        //if we have a target that is moving (and is not us), wait until it comes to the position
        while(newAction.targeted && targetPos && targetPos.Pos.Entity != actionComponent.Entity && targetPos.Moving) {yield return null;} 

        //turn off old action
        if (actionEffect != null && newAction.Action != Action) { ToggleAction(false, actionEffect); }
        ToggleAction(true, newAction);

        yield return new WaitForSeconds(2f);
        
        ToggleAction(false, newAction);

    }

    public virtual void ToggleAction(bool toggle, ActionEffect newEffect) {

        actionEffect = toggle ? newEffect : null;

        //play new action if it exists
        if(newEffect == null) return;
        newEffect.Toggle(toggle, this);

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

    void OnDrawGizmos() {
        if(targetPos) {
            Gizmos.color = targetPos.Moving ? Color.yellow : Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * .2f, targetPos.transform.position + Vector3.up * .2f);
        }
        
        if(actionComponent && actionComponent.Action != ActionName.None) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up * .1f, actionComponent.Position + Vector3.up * .1f);
            Gizmos.DrawWireSphere(actionComponent.Position + Vector3.up * .1f, .1f);
        }
    }
  

}
