using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;

public class SoldierComponent : MoverComponent
{
    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Soldier");
    }
    protected override IMudTable GetTable() {return new SoldierTable();}

}
