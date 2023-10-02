using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class Stick : Equipment
{

    public override bool CanUse() {
        bool canUse = base.CanUse();

        MUDEntity e = CursorMUD.MUDEntity;
        if(e == null) return false;
        
        PlayerComponent player = e?.GetMUDComponent<PlayerComponent>();
        MoveComponent moveType = e?.GetMUDComponent<MoveComponent>();
        bool onBounds = BoundsComponent.OnBounds((int)transform.position.x, (int)transform.position.z);
        return canUse && onBounds && player != null && !player.IsLocalPlayer && moveType != null && moveType.MoveType == MoveType.Push; 
    }
    
    public override async UniTask<bool> Use() {
        
        Debug.Log("PUSHING");

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = transform.position + (transform.position - Sender.transform.position).normalized;

        if(!BoundsComponent.OnBounds((int)pushToPos.x, (int)pushToPos.z)) {
            BoundsComponent.ShowBorder();
            return false;
        }

        List<TxUpdate> updates = new List<TxUpdate>();

        PositionComponent ourPosition = Sender.GetComponent<PlayerMUD>().Position;
        PositionComponent theirPosition = CursorMUD.MUDEntity.GetMUDComponent<PositionComponent>();

        updates.Add(TxManager.MakeOptimistic(ourPosition, PositionComponent.PositionToOptimistic(pushObject)));
        updates.Add(TxManager.MakeOptimistic(theirPosition, PositionComponent.PositionToOptimistic(pushToPos)));

        return await ActionsMUD.ActionTx(ourComponent.Entity, ActionName.Stick, pushObject, updates);
    }
    
}
