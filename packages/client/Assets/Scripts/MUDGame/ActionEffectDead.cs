using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectDead : ActionEffect
{

    protected override void ToggleActionEffects(bool toggle) {
        base.ToggleActionEffects(toggle);
        anim.ToggleRagdoll(true);
    }
}
