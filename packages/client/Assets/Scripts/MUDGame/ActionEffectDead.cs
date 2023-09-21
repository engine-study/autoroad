using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectDead : ActionEffect
{
    public override void Toggle(bool toggle, AnimationMUD animation) {
        base.Toggle(toggle, animation);

        animation.ToggleRagdoll(toggle);
    }

}