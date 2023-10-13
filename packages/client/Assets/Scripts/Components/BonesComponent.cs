using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class BonesComponent : MUDComponent
{
    protected override IMudTable GetTable() {return new BonesTable();}
    protected override void UpdateComponent(mud.IMudTable table, UpdateInfo newInfo) {

    }

}
