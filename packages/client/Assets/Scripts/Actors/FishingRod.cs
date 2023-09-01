using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud.Unity;
public class FishingRod : Equipment
{
    
    public override bool CanUse() {
        bool canUse = base.CanUse();

        MUDEntity e = CursorMUD.MUDEntity;
        MoveComponent moveType = e?.GetMUDComponent<MoveComponent>();

        bool onBounds = BoundsComponent.OnBounds(CursorMUD.GridPos);

        return canUse && onBounds && moveType != null && moveType.MoveType == MoveType.Push;
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
