using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum MoveType { None, Obstruction, Shovel, Carry, Push } 
public class MoveComponent : MUDComponent {
    public MoveType moveType;

    protected override IMudTable GetTable() { return new MoveTable(); }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        MoveTable table = (MoveTable)update;
        moveType = (MoveType)table.value;

    }
}
