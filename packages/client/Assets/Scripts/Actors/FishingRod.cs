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

        MUDEntity e = GridMUD.GetEntityAt(transform.position);
        MoveComponent moveType = e?.GetMUDComponent<MoveComponent>();
        PlayerComponent player = e?.GetMUDComponent<PlayerComponent>();

        bool onBounds = BoundsComponent.OnBounds((int)transform.position.x, (int)transform.position.z);

        return canUse && onBounds && player != null && player.IsLocalPlayer == false && moveType != null && moveType.MoveType == MoveType.Push;
    }
    
    public override async UniTask<bool> Use() {
        
        Debug.Log("PUSHING");

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = transform.position + (Sender.transform.position - transform.position).normalized;

        if(!BoundsComponent.OnBounds((int)pushToPos.x, (int)pushToPos.z)) {
            BoundsComponent.ShowBorder();
            return false;
        }

        List<TxUpdate> updates = new List<TxUpdate>();
        PositionComponent theirPosition = CursorMUD.MUDEntity.GetMUDComponent<PositionComponent>();

        updates.Add(TxManager.MakeOptimistic(theirPosition, (int)pushToPos.x, (int)pushToPos.z));

        return await TxManager.Send<FishFunction>(updates, System.Convert.ToInt32(pushObject.x), System.Convert.ToInt32(pushObject.z), System.Convert.ToInt32(pushToPos.x), System.Convert.ToInt32(pushToPos.z));
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
