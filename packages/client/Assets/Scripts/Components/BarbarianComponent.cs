using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;

public class BarbarianComponent : MoverComponent
{
    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Barbarian");
    }
    protected override IMudTable GetTable() {return new BarbarianTable();}

}
