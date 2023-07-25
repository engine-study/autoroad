using UnityEngine;
using System.Collections.Generic;
using mud.Unity;
using IWorld.ContractDefinition;
using DefaultNamespace;
using mud.Client;

public class InteractShovel : SPInteract
{

    public override void Interact(bool toggle, IActor newActor)
    {
        base.Interact(toggle, newActor);

        if(toggle) {
            Shovel();
        }
    }

    public void Shovel() {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;
        //no way of optimistic spawning yet
        string entity = MUDHelper.Keccak256("Road", x,y);
        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        updates.Add(TxManager.MakeOptimisticInsert<RoadComponent>(entity, 1));
        TxManager.Send<ShovelFunction>(updates, System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }

}
