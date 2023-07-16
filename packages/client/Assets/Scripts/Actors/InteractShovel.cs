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
        //no way of optimistic spawning yet
        // TxManager.MakeOptimistic

        TxManager.Send<ShovelFunction>(System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }

}
