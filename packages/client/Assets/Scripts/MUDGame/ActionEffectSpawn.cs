using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectSpawn : ActionEffect
{
    
    protected override void ToggleActionEffects(bool toggle) {
        base.ToggleActionEffects(toggle);
        anim.ToggleRagdoll(false);
    }
}
