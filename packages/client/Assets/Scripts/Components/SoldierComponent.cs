using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class SoldierComponent : MoverComponent
{
    protected override void PostInit() {
        base.PostInit();
        if(ActiveTable is SoldierTable) {
            Entity.SetName("Militus");
        } 
    }

}
