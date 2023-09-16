using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class MiliariumComponent : MUDComponent {

    protected override void OnDestroy() {
        base.OnDestroy();

    }

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Milliarium");
    }
    
    protected override IMudTable GetTable() {return new MiliariumTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        MiliariumTable table = update as MiliariumTable;
    }

}
