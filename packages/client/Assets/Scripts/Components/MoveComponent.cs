using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum MoveType {None, Shovel, Carry, Push} //"None", "Shovel", "Carry", "Push"
public class MoveComponent : MUDComponent
{
    public MoveType moveType;

    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType)
    {
        base.UpdateComponent(update, eventType);

        MoveTable table = (MoveTable)update;

        moveType = table.value != null ? (MoveType)table.value : MoveType.None;

    }
}
