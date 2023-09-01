using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class AnimationPlayerMUD : AnimationMUD
{
    [Header("Player")]
    PlayerMUD playerScript;

    protected override void Awake() {
        base.Awake();
        playerScript = GetComponent<PlayerMUD>();
    }

    public override void ToggleAction(bool toggle, ActionEffect newAction) {
        base.ToggleAction(toggle, newAction);

        // if (playerScript.IsLocalPlayer) {
        //     //stop the player from looking at the cursor when theyre moving
        //     playerScript.Animator.IK.SetLook(null);
        // }

        if(!playerScript.IsLocalPlayer) {
            playerScript.Actions.ActionToActionProp(Action, ActionComponent.Position);
        }


    }
}
