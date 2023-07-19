using UnityEngine;
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
        TxManager.MakeOptimisticInsert<RoadComponent>(MUDHelper.Keccak256("Road", x,y), 1);
        TxManager.MakeOptimisticInsert<PositionComponent>(MUDHelper.Keccak256("Road", x,y), x,y);
        TxManager.Send<ShovelFunction>(System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }

}
