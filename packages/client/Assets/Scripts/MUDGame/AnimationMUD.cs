using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using Edelweiss.Coroutine;

public class AnimationMUD : MonoBehaviour
{
    public ActionName Action {get{return actionEffect ? actionEffect.Action : ActionName.Idle;}}
    public ActionComponent ActionData {get{return actionData;}}
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
    [SerializeField] ActionName actionName;
    [SerializeField] ActionEffect actionEffect;

    [Header("Linked")]
    public PositionSync PositionSync;
    public SPAnimator Animator;
    public SPController Controller;
    [SerializeField] MUDEntity entity;
    [SerializeField] MUDComponent ourComponent;
    [SerializeField] ActionComponent actionData;
    [SerializeField] SPLooker looker;
    [SerializeField] IActor actor;

    protected virtual void Awake() {
        if(root == null) root = transform;

        entity = GetComponentInParent<MUDEntity>();
        looker = root.gameObject.AddComponent<SPLooker>();

        ourComponent = GetComponent<MUDComponent>();
        // ourComponent.OnToggle += ToggleDead;
    }

    protected virtual void Start() {

        if(entity == null) return;

        PositionSync = GetComponentInParent<PositionSync>(true);

        Controller = GetComponentInChildren<SPController>(true);
        if(Controller == null) {
            Controller = gameObject.AddComponent<SPController>();
            Controller.Init();
            Controller.ToggleController(false);
        }

        Animator = GetComponentInChildren<SPAnimator>(true);
        if(Animator) {
            head = Animator.Head;
            headRB = head.GetComponent<Rigidbody>();
            headParent = head.transform.parent;
            headPosLocal = head.localPosition;
            headRotLocal = head.localRotation;
            actor = new ActorAnimator(Animator);
        }

        actionData = MUDWorld.FindOrMakeComponent<ActionComponent>(entity.Key);

        if(entity.Loaded) Init();
        else entity.OnLoaded += Init;
    }

    void Init() {
        actionData = entity.GetMUDComponent<ActionComponent>();
        if (actionData == null) { Debug.LogError("No action component", this); return; }

        actionData.OnUpdated += UpdateAction;
    }

    void OnDestroy() {

        if(entity) entity.OnLoaded -= Init;
        if(actionData) actionData.OnUpdated -= UpdateAction;
        // if(ourComponent) ourComponent.OnToggle -= ToggleDead;

    }

    void UpdateAction() {
        EnterState(actionData.Action);
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
            // head.parent = null;
            // headRB.isKinematic = true;
            // headRB.detectCollisions = false;
            // headRB.velocity = Random.onUnitSphere * .1f;
        } else {
            //set head back to where its supposed to be
            // headRB.isKinematic = true;
            // headRB.isKinematic = false;
            ToggleSimple(isSimple);
        }

    }

    public virtual void EnterState(ActionName newAction) {

        actionName = newAction;

        Debug.Log($"[ANIM]: {actionData.Entity.Name} [{newAction.ToString().ToUpper()}] ({(int)actionData.Position.x},{(int)actionData.Position.z})", this);
        ActionEffect newEffect = LoadAction(newAction.ToString());

        //look at the new thing
        looker?.SetLookRotation(actionData.Position);

        if(newEffect == null) {
            ToggleAction(false, actionEffect);
            actionEffect = null;
            return;
        }

        //check that the action is the right one
        if(newEffect.Action != newAction) {Debug.LogError(newAction + " doesn't match on " + newEffect.name);}

        //setup the new movement type instantlye
        newEffect.ToggleMovement(true, this);

        if(Animation != null) actionData.Entity.StopCoroutine(Animation); 

        if(entity.IsLocal) {
            //assume we're already in the action because we had to have cast it?
            // Animation = StartCoroutine(AnimationInsanityLocal(newEffect));
            Animation = actionData.Entity.StartCoroutine(AnimationInsanity(newEffect));
        } else {
            //wait for target to move into place, then do animation
            Animation = actionData.Entity.StartCoroutine(AnimationInsanity(newEffect));
        }
        
    }   


    IEnumerator AnimationInsanityLocal(ActionEffect newAction) {

        ToggleAction(true, newAction);
        yield return null;
        ToggleAction(false, newAction);
        Animation = null;

    }

    Coroutine Animation;
    IEnumerator AnimationInsanity(ActionEffect newAction) {

        Debug.Log(actionData.Entity.Name + " START -------------", this);

        //wait a frame so all the positions are synced
        yield return null;

        //if we have a target that is moving (and is not us), wait until it comes into the same grid as the position
        while(actionData.Target && actionData.Target.GridPos != actionData.Position) {yield return null;} 

        //turn off old action
        if (actionEffect != null && newAction.Action != Action) { ToggleAction(false, actionEffect); }

        Debug.Log(actionData.Entity.Name + " TOGGLE -------------", this);

        //turn on new action
        ToggleAction(true, newAction);

        //let it play for a couple seconds
        yield return new WaitForSeconds(entity.IsLocal ? 2f : 2f);
        
        Debug.Log(actionData.Entity.Name + " END -------------", this);

        ToggleAction(false, newAction);

        Animation = null;
    }

    public virtual void ToggleMovement(bool toggle, ActionEffect newEffect) {
        //play new action if it exists
        if(newEffect == null) return;
        newEffect.ToggleMovement(toggle, this);
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

            GameObject resource = Resources.Load<GameObject>("Action/" + action);
            if(resource == null) return null;

            newAction = Instantiate(resource, transform).GetComponent<ActionEffect>();
            newAction.name = newAction.Action.ToString();
            newAction.transform.localPosition = Vector3.zero;
            newAction.transform.localRotation = Quaternion.identity;
            
            effects.Add(action, newAction);
        }

        return newAction;
    }

    void OnDrawGizmos() {
        if(actionData && Animation != null) {
        
            // if(actionData.Action != ActionName.None && actionData.Position != transform.position) {
            //     Gizmos.color = Color.green;
            //     Gizmos.DrawLine(transform.position + Vector3.up * .2f, actionData.Position + Vector3.up * .2f);    
            // }
            
            if(actionData && actionData.Target) {
                Gizmos.color = actionData.Target.Moving ? Color.blue : Color.green;
                Gizmos.DrawWireCube(actionData.Target.GridPos, Vector3.one * .5f - Vector3.up * .48f);
                Gizmos.DrawLine(transform.position + Vector3.up * .15f, actionData.Target.transform.position + Vector3.up * .15f);
                Gizmos.DrawWireSphere(actionData.Target.Pos.Pos + Vector3.up * .15f, .1f);
            }

        }
    }
  

}
