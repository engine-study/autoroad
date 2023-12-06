using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using Edelweiss.Coroutine;

public class AnimationMUD : MonoBehaviour
{
    public bool IsMove(ActionName action) {return action == ActionName.Walking || action == ActionName.Push || action == ActionName.Hop || action == ActionName.Teleport || action == ActionName.Swap || action == ActionName.Spawn;}

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
    public List<ActionTable> ActionQueue;
    [SerializeField] ActionTable actionTable;
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

        ActionQueue = new List<ActionTable>();

        entity = GetComponentInParent<MUDEntity>();
        looker = root.gameObject.AddComponent<SPLooker>();

        ourComponent = GetComponent<MUDComponent>();
        // ourComponent.OnToggle += ToggleDead;
    }

    protected virtual void Start() {

        if(entity == null) return;

        PositionSync = GetComponentInParent<PositionSync>(true);
        PositionSync.overrideSync = true;

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

        actionData = MUDWorld.FindOrMakeComponent<ActionTable, ActionComponent>(entity);

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
        IngestState(actionData.ActiveTable as ActionTable);
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

    public virtual void IngestState(ActionTable newAction) {

        ActionQueue.Add(newAction);

        //start the coroutine again
        if(ActionQueue.Count == 1) {
            actionData.Entity.StartCoroutine(ActionQueueCoroutine());
        }
    }



    public virtual Coroutine EnterState(ActionTable newAction) {

        actionTable = newAction;

        ActionName actionType = (ActionName)newAction.Action;
        Vector3 position = new Vector3((int)newAction.X, 0f, (int)newAction.Y);
        ActionEffect newEffect = LoadAction(actionType.ToString());

        //error checks
        Debug.Log($"[ANIM]: {actionData.Entity.Name} [{newAction.ToString().ToUpper()}] ({(int)position.x},{(int)position.z})", this);
        if(newEffect == null) { Debug.LogError($"{actionType} not handled"); return null;}
        if(newEffect.Action != actionType) {Debug.LogError(newAction + " doesn't match on " + newEffect.name);}

        //setup movement
        ToggleMovement(true, newEffect); 
        
        //do move if it is a move
        looker?.SetLookRotation(position);
        if(IsMove(actionType)) { PositionSync.StartMove(position); }

        if(entity.gameObject.activeInHierarchy) {
            //wait for target to move into place, then do animation
            if(Animation != null) actionData.Entity.StopCoroutine(Animation); 
            Animation = actionData.Entity.StartCoroutine(DoAction(newEffect));
            return Animation;
        } else {
            ToggleAction(true, newEffect);
            return null;
        }
        
    }   

    IEnumerator ActionQueueCoroutine() {
        while(ActionQueue.Count > 0) {
            yield return EnterState(ActionQueue[0]);
            ActionQueue.RemoveAt(0);
        }
    }


    Coroutine Animation;
    IEnumerator DoAction(ActionEffect newAction) {

        Debug.Log(actionData.Entity.Name + " START -------------", this);

        //wait a frame so all the positions are synced
        yield return null;
        while(PositionSync.Moving) {yield return null;}

        //if we have a target that is moving (and is not us), wait until it comes into the same grid as the position
        while(actionData.Target && actionData.Target.GridPos != actionData.Position) {yield return null;} 

        //turn off old action
        if (actionEffect != null && newAction.Action != Action) { ToggleAction(false, actionEffect); }

        Debug.Log(actionData.Entity.Name + " TOGGLE -------------", this);

        //turn on new action
        ToggleAction(true, newAction);

        //let it play for a couple seconds
        if(IsMove(actionEffect.Action) == false) {
            yield return new WaitForSeconds(entity.IsLocal ? 1.5f : 1.5f);
        }
        
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

            #if UNITY_EDITOR
            GUIStyle style = new GUIStyle();
            style.fontSize = 18;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleCenter;
            
            for(int i = 0; i < ActionQueue.Count; i++) {

                Color color =  Color.Lerp(Color.yellow, Color.blue, i/(float)ActionQueue.Count);
                Gizmos.color = color;
                style.normal.textColor = color;
           

                Vector3 waypoint = new Vector3((int)ActionQueue[i].X, 0.5f, (int)ActionQueue[i].Y);
                Gizmos.DrawWireSphere(waypoint, .25f);
                UnityEditor.Handles.Label(waypoint + Vector3.up * .5f, ((ActionName)ActionQueue[i].Action).ToString(), style);
                if(i > 0) {
                    Vector3 from = new Vector3((int)ActionQueue[i-1].X, 0.5f, (int)ActionQueue[i-1].Y);
                    Gizmos.DrawLine(from, waypoint);
                }
            }
            #endif
            
            if(actionData && actionData.Target) {
                Gizmos.color = actionData.Target.Moving ? Color.blue : Color.green;
                Gizmos.DrawWireCube(actionData.Target.GridPos, Vector3.one * .5f - Vector3.up * .48f);
                Gizmos.DrawLine(transform.position + Vector3.up * .15f, actionData.Target.transform.position + Vector3.up * .15f);
                Gizmos.DrawWireSphere(actionData.Target.Pos.Pos + Vector3.up * .15f, .1f);
            }

        }
    }
  

}
