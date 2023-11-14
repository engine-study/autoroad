using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class BonesComponent : MUDComponent
{
    protected override MUDTable GetTable() {return new BonesTable();}
    protected override void UpdateComponent(mud.MUDTable table, UpdateInfo newInfo) {

    }

}
