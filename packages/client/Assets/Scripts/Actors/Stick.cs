using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class Stick : Equipment
{

    public override bool CanUse() {
        bool canUse = base.CanUse();

        MUDEntity e = CursorMUD.Entity;
        if(e == null) return false;
        
        NPCComponent npc = e.GetMUDComponent<NPCComponent>();
        MoveComponent moveType = e.GetMUDComponent<MoveComponent>();

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = transform.position + (transform.position - Sender.transform.position).normalized;

        bool onBounds = OnWorld(e, pushToPos);
        return canUse && onBounds && npc != null && moveType != null && moveType.MoveType == MoveType.Push; 
    }
    
    public override async UniTask<bool> Use() {
        
        Debug.Log("STICKING");

        // List<TxUpdate> updates = new List<TxUpdate>();

        // PositionComponent ourPosition = Sender.GetComponent<PlayerMUD>().Position;
        // PositionComponent theirPosition = CursorMUD.Entity.GetMUDComponent<PositionComponent>();

        // updates.Add(TxManager.MakeOptimistic(ourPosition, PositionComponent.PositionToOptimistic(pushObject)));
        // updates.Add(TxManager.MakeOptimistic(theirPosition, PositionComponent.PositionToOptimistic(pushToPos)));

        return await ActionsMUD.ActionTx(ourComponent.Entity, ActionName.Stick, transform.position);
    }
    
}
