using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class TriggerComponent : MUDComponent {

    protected override void OnDestroy() {
        base.OnDestroy();

    }

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Milliarium Puzzle");
    }
    
    protected override IMudTable GetTable() {return new TriggerTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        TriggerTable table = update as TriggerTable;
    }

}
