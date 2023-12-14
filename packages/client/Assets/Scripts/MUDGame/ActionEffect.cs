using UnityEngine;
using mudworld;

public class ActionEffect : MonoBehaviour {
    

    public ActionName Action {get{return actionName;}}
    public SPAction ActionData {get{return action;}}
    [Header("Action")]
    [SerializeField] ActionName actionName;
    public Vector3 position;

    [Header("Movement")]
    public MoverMUD movement;
    public SPEnableDisable moveEffect;
    public string movementClip;

    [Header("Action")]
    public bool targeted;
    [SerializeField] SPAction action;
    [SerializeField] SPEnableDisable effect;
    [SerializeField] string actionClip;

    [Header("Debug")]
    [SerializeField] bool active = false; 
    [SerializeField] protected AnimationMUD anim;

    void Awake() {
        if(effect) effect.ToggleActive(false);
        if(moveEffect) moveEffect.ToggleActive(false);
    }
    
    public virtual void Toggle(bool toggle, AnimationMUD animation) {

        anim = animation;
        if(movement) animation.PositionSync.SetMovement(movement);

        Debug.Log($"[EFFECT_TOGGLE {toggle}]: {anim.ActionData.Entity.Name} [{gameObject.name}]", this);

        //first time setup
        if (active != toggle) {

            if (toggle) { anim.PositionSync.OnMoveEnd += OnMoveEnd;}  
            else {anim.PositionSync.OnMoveEnd -= OnMoveEnd; }
        }

        if(toggle) {

            //move to the position of our action's movement if its a move action
            animation.Looker?.SetLookRotation(position);
            if(AnimationMUD.IsMove(Action)) {
                animation.PositionSync.StartMove(position);
                ToggleMovementEffects(true);
            } else {
                ToggleActionEffects(true);
            }
            
        } else {
            ToggleActionEffects(false);
        }

        active = toggle;
        gameObject.SetActive(toggle);

    }
    
    public void OnMoveStart() {
        ToggleMovementEffects(true);
    }   

    public void OnMoveEnd() {
        ToggleMovementEffects(false);
    }


    protected virtual void ToggleMovementEffects(bool toggle) {

        Debug.Log($"[EFFECT_MOVE {toggle}]: {anim.ActionData.Entity.Name} [{gameObject.name}] ", this);

        if(toggle) {
            moveEffect?.Spawn(true);
            PlayAnimation(movementClip);
        } else {
            moveEffect?.Spawn(false);
            ToggleActionEffects(true);
        }

    }

    protected virtual void ToggleActionEffects(bool toggle) {

        Debug.Log($"[EFFECT_ACTION {toggle}]: {anim.ActionData.Entity.Name} [{gameObject.name}]", this);

        if(toggle) {

            effect?.Spawn(true);
            PlayAnimation(actionClip);

        } else {
        
            effect?.Spawn(false);
            // PlayAnimation("");
        }

        if(action && anim) {
            action.DoCast(toggle, anim.Actor);
        }
    }

    void PlayAnimation(string clipName) {        
        if(anim.Animator == null) { return; }
        if (string.IsNullOrEmpty(clipName)) { return; }
        anim.Animator.PlayClip(clipName);
    }

}
