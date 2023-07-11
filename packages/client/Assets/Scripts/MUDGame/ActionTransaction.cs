using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using mud.Client;
using mud.Unity;
using DefaultNamespace;
using IWorld.ContractDefinition;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ActionTransaction", menuName = "Engine/Action/ActionTransaction", order = 1)]
public class ActionTransaction : SPAction
{
    [Header("MUD")]
    public string transactionName;

    
    public override void EndAction(IActor actor, IInteract interactable, ActionEndState reason) {
        base.EndAction(actor, interactable, reason);    

        if(reason != ActionEndState.Success) {
            return;
        }

        interactable.GameObject().BroadcastMessage(transactionName);

    }

    public void Shovel(IActor actor, IInteract interactable, ActionEndState reason) {
        ShovelAction((int)interactable.GameObject().transform.position.x, (int)interactable.GameObject().transform.position.y);
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
