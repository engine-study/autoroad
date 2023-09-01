using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffect : MonoBehaviour {
    [Header("Action Data")]
    public MoverMUD movement;
    public SPEnableDisable effect;
    public SPEnableDisable moveEffect;
    public SPAction action;
    public string animationClip;

    AnimationMUD setup;
    bool active = false; 
    public void Toggle(bool toggle, AnimationMUD animation) {

        if(toggle != active) {
            Setup(toggle, animation);
        }

        active = toggle;
        setup = toggle ? animation : null;

        //we're already active
        if(gameObject.activeInHierarchy) {
            if(effect) {
                if(toggle) { effect.PlayEnabled();
                } else {effect.PlayDisabled();}
            }
        }

        if (animation.PositionSync) {
            if(toggle) {
                if (animation.PositionSync.Moving) { OnMoveStart(); }
            } else {
                if (animation.PositionSync.Moving) { OnMoveEnd(); }
            }
        }

        gameObject.SetActive(toggle);

        if(toggle) {

            if(animation.Animator) {
                if (!string.IsNullOrEmpty(animationClip)) {
                    animation.Animator.PlayClip(animationClip);
                }
                
                if(action) {
                    if(action is SPActionPlayer) {
                        SPActionPlayer actionPlayer = action as SPActionPlayer;
                        actionPlayer.animatorState?.Apply(animation.Animator);
                    }
                }
            }

        } else {

        }
    }

    void Setup(bool toggle, AnimationMUD animation) {
        if (animation.PositionSync) {
            if(toggle) {
                if (animation.PositionSync.Moving) { OnMoveStart(); }
                animation.PositionSync.OnMoveStart += OnMoveStart;
                animation.PositionSync.OnMoveEnd += OnMoveEnd;
                animation.PositionSync.SetMovement(movement);
            } else {
                if (animation.PositionSync.Moving) { OnMoveEnd(); }
                animation.PositionSync.OnMoveStart -= OnMoveStart;
                animation.PositionSync.OnMoveEnd -= OnMoveEnd;
            }
        }
    }

    public void OnMoveStart() {
        moveEffect?.PlayEnabled();
    }   

    public void OnMoveEnd() {
        moveEffect?.PlayDisabled();
    }

}
