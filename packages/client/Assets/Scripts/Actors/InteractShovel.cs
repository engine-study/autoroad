using UnityEngine;
using mud.Unity;
using IWorld.ContractDefinition;

public class InteractShovel : SPInteract
{

    public override void Interact(bool toggle, IActor newActor)
    {
        base.Interact(toggle, newActor);

        if(toggle) {
            ShovelAction((int)transform.position.x, (int)transform.position.y);
        }
    }

    public async void ShovelAction(int x, int y)
    {
        try
        {
            // function moveFrom(int32 startX, int32 startY, int32 x, int32 y) public {
            await NetworkManager.Instance.worldSend.TxExecute<ShovelFunction>(x, y);
        }
        catch (System.Exception ex)
        {
            //if our transaction fails, force the player back to their position on the table
            Debug.LogException(ex);
        }
    }

}
