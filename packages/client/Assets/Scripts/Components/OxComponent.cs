using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public class OxComponent : MoverComponent {

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Ox");
    }

    protected override void InitDestroy() {
        base.InitDestroy();

    }

    protected override IMudTable GetTable() {return new OxTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo)
    {
        base.UpdateComponent(update, newInfo);
    }
}
