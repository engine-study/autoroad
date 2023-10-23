using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud;
public class FishingRod : Equipment {
    
    public override bool IsInteractable() {
        bool canUse = base.IsInteractable();

        MUDEntity e = CursorMUD.Entity;

        if(e == null) return false;
        
        MoveComponent moveType = e.GetMUDComponent<MoveComponent>();
        WeightComponent weight = e.GetMUDComponent<WeightComponent>();

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = Sender.transform.position - (transform.position - Sender.transform.position).normalized;

        if(!PositionComponent.OnWorldOrMap(e, pushToPos)) {return false;}

        return canUse && moveType != null && moveType.MoveType == MoveType.Push && (weight == null || weight.Weight <= 0);
    }
    
    public override async UniTask<bool> SendTx() {
        
        Debug.Log("Fishing");

        // PositionComponent theirPosition = CursorMUD.Entity.GetMUDComponent<PositionComponent>();

        // List<TxUpdate> updates = new List<TxUpdate>();
        // updates.Add(ActionsMUD.ActionOptimistic(CursorMUD.Entity, ActionName.Hop, pushToPos));
        // updates.Add(ActionsMUD.PositionOptimistic(CursorMUD.Entity, pushToPos));
        
        return await ActionsMUD.ActionTx(Entity, ActionName.Fishing, CursorMUD.GridPos);

    }

    
    // public void Fish() {

    //     MUDEntity e = CursorMUD.Entity as MUDEntity;
    //     string entity = e.Key;

    //     PositionComponent pos = e.GetMUDComponent<PositionComponent>();
    //     // string entityAddress = MUDHelper.EntityKeyToBytes32(entity);
        
    //     Vector3 pullToPlayer = pos.Pos + (player.Position.Pos - pos.Pos).normalized;
        
    //     List<TxUpdate> updates = new List<TxUpdate>();
    //     updates.Add(TxManager.MakeOptimistic(pos, (int)pullToPlayer.x, (int)pullToPlayer.z));
    //     SendPositionFunction<FishFunction>(updates, FishAction);
    // }
}
