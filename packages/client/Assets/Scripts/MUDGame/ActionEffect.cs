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
        if(moveEffect) moveEffect.active = false;
    }
    
    public void Toggle(bool toggle, AnimationMUD animation) {

        setup = animation;

        //first time setup
        if (active != toggle) {

            if (setup.PositionSync) {
                if (toggle) {
                    setup.PositionSync.OnMoveStart += OnMoveStart;
                    setup.PositionSync.OnMoveEnd += OnMoveEnd;
                    setup.PositionSync.SetMovement(movement);
                }  else {
                    setup.PositionSync.OnMoveStart -= OnMoveStart;
                    setup.PositionSync.OnMoveEnd -= OnMoveEnd;
                }
            }
        }

        //play state animation
        ToggleActionAnimation(toggle);

        //play movement animation
        if(setup.PositionSync) {
            if(setup.PositionSync.Moving) {
                PlayAnimation(movementClip);
            }
        }

        active = toggle;
        gameObject.SetActive(toggle);


    }

    void ToggleActionAnimation(bool toggle) {

        if(toggle) {
            PlayAnimation(actionClip);
            //ugly hack
            if(action) {
                if(action is SPActionPlayer) {
                    SPActionPlayer actionPlayer = action as SPActionPlayer;
                    actionPlayer.animatorState?.Apply(setup.Animator);
                }
            }

        } else {
            PlayAnimation("");
        }
    }

    void PlayAnimation(string clipName) {        
        if(setup.Animator == null) { return; }
        if (string.IsNullOrEmpty(clipName)) { return; }
        setup.Animator.PlayClip(clipName);
    }

    public void OnMoveStart() {
        moveEffect?.PlayEnabled();
        PlayAnimation(movementClip);
    }   

    public void OnMoveEnd() {
        moveEffect?.PlayDisabled();
        PlayAnimation(actionClip);
    }

}
