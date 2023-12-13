using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
public class OxComponent : MoverNPCComponent {

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Chaddius");
    }

    protected override void InitDestroy() {
        base.InitDestroy();

    }

    protected override MUDTable GetTable() {return new OxTable();}
    protected override void UpdateComponent(mud.MUDTable update, UpdateInfo newInfo)
    {
        base.UpdateComponent(update, newInfo);
    }
}
