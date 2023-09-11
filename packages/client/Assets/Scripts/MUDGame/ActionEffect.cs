using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour {
    [Header("Action Data")]
    public MoverMUD movement;
    public SPEnableDisable effect;
    public SPEnableDisable moveEffect;
    public SPAction action;
    public string actionClip;
    public string movementClip;

    AnimationMUD setup;
    bool active = false; 

    void Awake() {
        if(effect) effect.active = false;
        if(moveEffect) moveEffect.active = false;
    }
    
    public void Toggle(bool toggle, AnimationMUD animation) {

        setup = animation;

        //first time setup
        if (active != toggle) {
            if (toggle) {
                setup.PositionSync.OnMoveEnd += OnMoveEnd;
                setup.PositionSync.SetMovement(movement);
            }  else {
                setup.PositionSync.OnMoveEnd -= OnMoveEnd;
            }
            
        }


        if(toggle) {
            
            //set the movement, LET THIS LINGER, states without movement use the last movement given
            setup.PositionSync.SetMovement(movement);

            if(setup.PositionSync.Moving) {
                //play movement animation
                ToggleMoveAnimation(true);
            } else {
                //play state animation
                ToggleActionAnimation(true);
            }
        } else {
            ToggleActionAnimation(false);
        }

        active = toggle;
        gameObject.SetActive(toggle);

    }
    
    public void OnMoveStart() {
        ToggleMoveAnimation(true);
    }   

    public void OnMoveEnd() {
        ToggleMoveAnimation(false);
    }


    void ToggleMoveAnimation(bool toggle) {

        if(toggle) {
            moveEffect?.PlayEnabled();
            PlayAnimation(movementClip);
        } else {
            moveEffect?.PlayDisabled();
            ToggleActionAnimation(true);
    
        }

    }

    void ToggleActionAnimation(bool toggle) {

        if(toggle) {

            effect?.PlayEnabled();
            PlayAnimation(actionClip);

            //move to AnimationPlayerMUD later
            if(action) {
                if(action is SPActionPlayer) {
                    SPActionPlayer actionPlayer = action as SPActionPlayer;
                    actionPlayer.animatorState?.Apply(setup.Animator);
                }
            }

        } else {
        
            effect?.PlayDisabled();
            // PlayAnimation("");
        }
    }

    void PlayAnimation(string clipName) {        
        if(setup.Animator == null) { return; }
        if (string.IsNullOrEmpty(clipName)) { return; }
        setup.Animator.PlayClip(clipName);
    }

}
