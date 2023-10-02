using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class FishingRod : Equipment
{
    
    public override bool CanUse() {
        bool canUse = base.CanUse();

        MUDEntity e = CursorMUD.MUDEntity;

        if(e == null) return false;
        
        MoveComponent moveType = e?.GetMUDComponent<MoveComponent>();
        WeightComponent weight = e?.GetMUDComponent<WeightComponent>();

        bool startOnBounds = BoundsComponent.OnWorld(e,CursorMUD.GridPos);
        bool endOnBounds = BoundsComponent.OnWorld(e,CursorMUD.GridPos);

        return canUse && startOnBounds && endOnBounds && moveType != null && moveType.MoveType == MoveType.Push && (weight == null || weight.Weight <= 0);
    }
    
    public override async UniTask<bool> Use() {
        
        Debug.Log("Fishing");

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = Sender.transform.position - (transform.position - Sender.transform.position).normalized;

        if(!BoundsComponent.OnBounds((int)pushToPos.x, (int)pushToPos.z)) {
            BoundsComponent.ShowBorder();
            return false;
        }

        PositionComponent theirPosition = CursorMUD.MUDEntity.GetMUDComponent<PositionComponent>();

        List<TxUpdate> updates = new List<TxUpdate>();
        updates.Add(ActionsMUD.ActionOptimistic(CursorMUD.MUDEntity, ActionName.Hop, pushToPos));
        updates.Add(ActionsMUD.PositionOptimistic(CursorMUD.MUDEntity, pushToPos));
        
        return await ActionsMUD.ActionTx(ourComponent.Entity, ActionName.Fishing, CursorMUD.GridPos);

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
