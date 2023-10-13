using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;

public class ArcherComponent : MUDComponent
{

    [Header("Archer")]
    [SerializeField] SPAnimationProp prop;

    protected override void PostInit() {
        base.PostInit();
        Entity.SetName("Archer");
    }

    protected override IMudTable GetTable() {return new ArcherTable();}
    protected override void UpdateComponent(IMudTable table, UpdateInfo newInfo) {}

}
