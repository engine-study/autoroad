using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;

public class Plant : Equipment
{
    
    public override bool IsInteractable() 
    {
        bool canUse = base.IsInteractable();
        bool onBounds = PositionComponent.OnMap(transform.position);
        // bool onRoad = RoadConfigComponent.OnRoad((int)transform.position.x, (int)transform.position.z);

        return canUse && SeedComponent.LocalCount > 0 && onBounds && CursorMUD.Entity == null;

    }

    
    public override async UniTask<bool> SendTx() {
        
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        //can try optimistic

        return await ActionsMUD.ActionTx(Entity, ActionName.Plant, new Vector3(x, 0, y));
    }

}
