using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour {
    
    [Header("Debug")]
    [SerializeField] bool active = false; 

    [Header("Movement")]
    public MoverMUD movement;
    public SPEnableDisable moveEffect;
    public string movementClip;

    [Header("Action")]
    public SPAction action;
    public SPEnableDisable effect;
    public string actionClip;

    [Header("Debug")]
    [SerializeField] protected AnimationMUD anim;

    void Awake() {
        if(effect) effect.active = false;
        if(moveEffect) moveEffect.active = false;
    }
    
    public virtual void Toggle(bool toggle, AnimationMUD animation) {

        anim = animation;

        //first time setup
        if (active != toggle) {
            if (toggle) { anim.PositionSync.OnMoveEnd += OnMoveEnd;}  
            else {anim.PositionSync.OnMoveEnd -= OnMoveEnd; }
        }

        if(toggle) {
            
            //set the movement, LET THIS LINGER, states without movement use the last movement given
            if(movement) anim.PositionSync.SetMovement(movement);

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

        if(toggle) {
            moveEffect?.PlayEnabled();
            PlayAnimation(movementClip);
        } else {
            moveEffect?.PlayDisabled();
            ToggleActionEffects(true);
    
        }

    }

    protected virtual void ToggleActionEffects(bool toggle) {

        Debug.Log(gameObject.name + ": " + toggle, this);

        if(toggle) {

            effect?.PlayEnabled();
            PlayAnimation(actionClip);

        } else {
        
            effect?.PlayDisabled();
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
