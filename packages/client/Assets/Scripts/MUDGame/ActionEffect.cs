using UnityEngine;
using mudworld;

public class ActionEffect : MonoBehaviour {
    

    public ActionName Action {get{return actionName;}}
    [Header("Action")]
    [SerializeField] ActionName actionName;

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
    
    public virtual void ToggleMovement(bool toggle, AnimationMUD animation) {

        //set the movement 
        if(movement) animation.PositionSync.SetMovement(movement);

    }

    public virtual void Toggle(bool toggle, AnimationMUD animation) {

        anim = animation;

        Debug.Log($"[A-Toggle]: {anim.ActionData.Entity.Name} [{gameObject.name}] {toggle}", this);

        //first time setup
        if (active != toggle) {
            if (toggle) { anim.PositionSync.OnMoveEnd += OnMoveEnd;}  
            else {anim.PositionSync.OnMoveEnd -= OnMoveEnd; }
        }

        if(toggle) {

            //first check if we are moving or not before performing action
            if(anim.PositionSync.Moving) {
                //play movement animation
                ToggleMovementEffects(true);
            } else {
                //play state animation
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

        Debug.Log($"[A-Move]: {anim.ActionData.Entity.Name} [{gameObject.name}] {toggle}", this);

        if(toggle) {
            moveEffect?.Spawn(true);
            PlayAnimation(movementClip);
        } else {
            moveEffect?.Spawn(false);
            ToggleActionEffects(true);
        }

    }

    protected virtual void ToggleActionEffects(bool toggle) {

        Debug.Log($"[A-Action]: {anim.ActionData.Entity.Name} [{gameObject.name}] {toggle}", this);

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
