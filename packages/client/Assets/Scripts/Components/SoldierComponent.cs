using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class SoldierComponent : MoverComponent
{
    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Soldier");
    }
    protected override MUDTable GetTable() {return new SoldierTable();}

}
