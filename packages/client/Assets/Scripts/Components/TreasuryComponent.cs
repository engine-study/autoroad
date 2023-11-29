using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class TreasuryComponent : MUDComponent
{
    protected override void UpdateComponent(MUDTable update, UpdateInfo info) {
        TreasuryTable table = (TreasuryTable)update;

        Entity.SetName("Treasury");

    }
}
