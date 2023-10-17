using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class CarriageComponent : MUDComponent
{
    protected override IMudTable GetTable() {return new CarriageTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {
        // throw new System.NotImplementedException();

        Entity.SetName("Carriage");
    }
}
