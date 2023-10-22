using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class EnableDisableMUD : SPEnableDisable
{

    [Header("MUD")]
    [SerializeField] MUDComponent component;

    //only fires from OnToggle
    public override bool CanFire(bool enable, SPEffects effect) { return false; }

    public override void ToggleActive(bool toggle) {
        base.ToggleActive(toggle);

        if(toggle) {

            if(component == null) {
                component = GetComponentInParent<MUDComponent>();
                if (component == null) { Debug.LogError("No component", this); return; }
            }

            component.OnToggleActive += Spawn;

        } else if(component) {

            component.OnToggleActive -= Spawn;

        }
        
    }
}
