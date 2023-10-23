using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class Shovel : Equipment
{
    public override bool IsInteractable() {
        canUse = base.IsInteractable() && RoadConfigComponent.OnRoad(transform.position) && CursorMUD.EntityUnder != null && CursorMUD.EntityUnder.GetMUDComponent<RoadComponent>()?.State == RoadState.Path; //distanceToPlayer > .5f && distanceToPlayer <= 1f &&
        return canUse;
    }

    public override async UniTask<bool> SendTx() {

        int x = (int)CursorMUD.GridPos.x;
        int y = (int)CursorMUD.GridPos.z;

        Debug.Log("Shoveled: " + x + "," + y);
        
        string entity = MUDHelper.Keccak256("Road", x,y);
        List<TxUpdate> updates = new List<TxUpdate>();
        
        // updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        // updates.Add(TxManager.MakeOptimisticInsert<RoadComponent>(entity, 1, NetworkManager.LocalAddress));
        
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, ActionName.Shoveling, new Vector3(x,0,y));
    }

}
