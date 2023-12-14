using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using Edelweiss.Coroutine;

public class AnimationMUD : MonoBehaviour
{
    public static bool IsMove(ActionName action) {return action == ActionName.Walking || action == ActionName.Push || action == ActionName.Hop || action == ActionName.Teleport || action == ActionName.Swap || action == ActionName.Spawn;}
    public static bool IsDisplace(ActionName action) {return action == ActionName.Push || action == ActionName.Fishing || action == ActionName.Throw || action == ActionName.Stick;}

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
    public List<ActionTable> ActionQueue;
    public ActionTable CurrentTable;
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
    bool hasInit = false; 

    protected virtual void Awake() {
        if(root == null) root = transform;

        ActionQueue = new List<ActionTable>();

        entity = GetComponentInParent<MUDEntity>();
        looker = root.gameObject.AddComponent<SPLooker>();
        float randomRot = Random.Range(0f,360f);
        randomRot = (int)(Mathf.Round(randomRot / 90f) * 90f);
        looker?.SetLookRotation(Quaternion.Euler(Vector3.up * randomRot));
        

        ourComponent = GetComponent<MUDComponent>();
        // ourComponent.OnToggle += ToggleDead;
    }

    void OnDisable() {
        if(ActionQueue.Count > 0 && ActionQueue[ActionQueue.Count-1] != CurrentTable) {
            IngestState(ActionQueue[ActionQueue.Count-1], true);
        }
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

        actionData = MUDWorld.FindOrMakeComponent<ActionTable, ActionComponent>(entity);

        if(entity.Loaded) Init();
        else entity.OnLoaded += Init;
    }

    void Init() {
        if(hasInit) {Debug.LogError("Double Init"); return;}

        actionData = entity.GetMUDComponent<ActionComponent>();
        if (actionData == null) { Debug.LogError("No action component", this); return; }

        actionData.OnUpdated += UpdateAction;
    
        hasInit = true;
    }

    void OnDestroy() {

        if(entity) entity.OnLoaded -= Init;
        if(actionData) actionData.OnUpdated -= UpdateAction;
        // if(ourComponent) ourComponent.OnToggle -= ToggleDead;

    }


    void UpdateAction() {
        //turn off positionsync
        PositionSync.overrideSync = true;
        PositionSync.hideAfterLoaded = false;
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
        
        if(Animator.IsHumanoid == false) return;

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

    Coroutine queue, animation;
    public virtual void IngestState(ActionTable newAction, bool instant = false) {
        
        bool startQueue = ActionQueue.Count == 0;
        ActionQueue.Add(newAction);

        Debug.Log($"[QUEUE]: {newAction.Action} [{ActionQueue.Count}]", this);

        //start the coroutine again
        if(entity.gameObject.activeInHierarchy && !instant) {
            if(startQueue) {
                if(queue != null) {actionData.Entity.StopCoroutine(queue);}
                if(animation != null) actionData.Entity.StopCoroutine(animation); 
                queue = actionData.Entity.StartCoroutine(ActionQueueCoroutine());
            }
        } else {
            LoadAction(newAction, true);
            ActionQueue = new List<ActionTable>();

        }
    }

    //process the queue
    IEnumerator ActionQueueCoroutine() {
        while(ActionQueue.Count > 0) {

            animation = actionData.Entity.StartCoroutine(EnterState(ActionQueue[0]));
            yield return animation;

            if(ActionQueue.Count > 0) {ActionQueue.RemoveAt(0);}
        }
    }

    //load the action, set movement
    public ActionEffect LoadAction(ActionTable table, bool instant = false) {
        
        CurrentTable = table;
        ActionName actionType = (ActionName)table.Action;
        Vector3 position = new Vector3((int)table.X, 0f, (int)table.Y);
        ActionEffect newEffect = LoadAction(actionType.ToString());

        //error checks
        Debug.Log($"[ANIM]: {actionData.Entity.Name} [{table.ToString().ToUpper()}] ({(int)position.x},{(int)position.z})", this);
        if(newEffect == null) { Debug.LogError($"{actionType} not handled"); return null;}
        if(newEffect.Action != actionType) {Debug.LogError(table + " doesn't match on " + newEffect.name);}

        //turn on new action
        ToggleAction(true, newEffect);

        //move to the position of our action's movement if its a move action
        //otherwise don't move us, but handle movement (so that necessary updates are sent through positionsync, like IsVisible)
        looker?.SetLookRotation(position);
        if(IsMove(actionType)) { PositionSync.StartMove(position); }
        else {PositionSync.StartMove(PositionSync.Target.position);}

        if(instant) {
            entity.Toggle(newEffect.Action != ActionName.Dead && newEffect.Action != ActionName.Destroy);
        }
        
        return newEffect;
    }

    public virtual void ToggleAction(bool toggle, ActionEffect newEffect) {

        actionEffect = toggle ? newEffect : null;
        actionName = newEffect ? newEffect.Action : ActionName.None;

        //play new action if it exists
        if(newEffect == null) return;
        newEffect.Toggle(toggle, this);

    }

    public virtual IEnumerator EnterState(ActionTable actionTable) {

        //wait a frame so position or other updates have settled
        yield return null;

        ActionEffect newEffect = LoadAction(actionTable);

        //wait a frame so all the positions are synced
        while(PositionSync.Moving) {yield return null;}

        if(entity.gameObject.activeInHierarchy) {
          
             Debug.Log(actionData.Entity.Name + " START -------------", this);

            //if we have a target that is moving (and is not us), wait until it comes into the same grid as the position
            bool waitForTarget = IsMove(newEffect.Action) == false && IsDisplace(newEffect.Action) == false;
            while(waitForTarget && actionData.Target && actionData.Target.GridPos != actionData.Position) {yield return null;} 

            Debug.Log(actionData.Entity.Name + " TOGGLE -------------", this);

            //let it play for a couple seconds
            if(IsMove(actionEffect.Action) == false) {
                yield return new WaitForSeconds(actionEffect.ActionData ? actionEffect.ActionData.CastDuration : 1f);
            }
            
            Debug.Log(actionData.Entity.Name + " END -------------", this);

            ToggleAction(false, newEffect);

        } else {
            ToggleAction(true, newEffect);
        }

        entity.Toggle(newEffect.Action != ActionName.Dead && newEffect.Action != ActionName.Destroy);
        
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

        if(!Application.isPlaying) {return;}

        #if UNITY_EDITOR
        GUIStyle style = new GUIStyle {
            fontSize = 18,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };

        for (int i = 0; i < ActionQueue.Count; i++) {

            Color color =  Color.Lerp(Color.cyan, Color.yellow, (i)/((float)ActionQueue.Count));
            Gizmos.color = color;
            style.normal.textColor = color;

            Vector3 waypoint = new Vector3((int)ActionQueue[i].X, 0.5f, (int)ActionQueue[i].Y);
            Gizmos.DrawSphere(waypoint, .05f);
            Vector3 from = i > 0 ? new Vector3((int)ActionQueue[i-1].X, 0.5f, (int)ActionQueue[i-1].Y) : PositionSync.Target.position + Vector3.up *.5f;
            Gizmos.DrawLine(from, waypoint);
            
            waypoint += Vector3.up * (i*.1f);
            UnityEditor.Handles.Label(waypoint + Vector3.up * .5f, ((ActionName)ActionQueue[i].Action).ToString(), style);
        }
        #endif

        if(actionData && animation != null) {

            if(actionData && actionData.Target) {
                Gizmos.color = actionData.Target.Moving ? Color.yellow : Color.cyan;
                Gizmos.DrawWireCube(actionData.Target.GridPos, Vector3.one * .5f - Vector3.up * .48f);
                Gizmos.DrawLine(transform.position + Vector3.up * .15f, actionData.Target.transform.position + Vector3.up * .15f);
                Gizmos.DrawWireSphere(actionData.Target.Pos.Pos + Vector3.up * .15f, .1f);
            }

        }
    }
  

}
