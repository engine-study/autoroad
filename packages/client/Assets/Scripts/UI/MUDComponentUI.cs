using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using System;

public abstract class MUDComponentUI : EntityUI
{
    [Header("Component")]
    public MUDComponent component;
    public abstract Type ComponentType();


    protected override void UpdateEntity() {
        base.UpdateEntity();

        component = Entity.GetMUDComponent(ComponentType());
        if(component == null) {
            ToggleWindowClose();
            return;
        }

        UpdateComponent();
    }

    protected virtual void UpdateComponent() {
        
    }

}
