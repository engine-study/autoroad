using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class ArcherComponent : MUDComponent
{

    [Header("Archer")]
    [SerializeField] SPAnimationProp prop;

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Archer");
    }

    protected override MUDTable GetTable() {return new ArcherTable();}
    protected override void UpdateComponent(MUDTable table, UpdateInfo newInfo) {}

}
