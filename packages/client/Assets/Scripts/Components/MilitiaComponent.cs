using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class MilitiaComponent : MoverComponent
{
    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Militia");
    }
    protected override IMudTable GetTable() {return new MilitiaTable();}

}
