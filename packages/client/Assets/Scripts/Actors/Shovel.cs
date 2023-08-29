using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud.Unity;

public class Shovel : Equipment
{
    public override bool CanUse() {
        bool canUse = base.CanUse();
        return canUse && GridMUD.GetEntityAt(CursorMUD.GridPos - Vector3.up) == null && RoadConfigComponent.OnRoad((int)transform.position.x, (int)transform.position.z) && CursorMUD.Entity == null; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
    }

    public override async UniTask<bool> Use() {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        Debug.Log("Shoveled: " + x + "," + y);
        
        string entity = MUDHelper.Keccak256("Road", x,y);
        List<TxUpdate> updates = new List<TxUpdate>();
        
        // updates.Add(TxManager.MakeOptimisticInsert<PositionComponent>(entity, x,y));
        // updates.Add(TxManager.MakeOptimisticInsert<RoadComponent>(entity, 1, NetworkManager.LocalAddress));
         
        return await TxManager.Send<ActionFunction>(updates, System.Convert.ToByte((int)StateType.Shoveling));
        return await TxManager.Send<ShovelFunction>(updates, System.Convert.ToInt32(x), System.Convert.ToInt32(y));
    }

}
