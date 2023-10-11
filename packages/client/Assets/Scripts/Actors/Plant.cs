using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;

public class Plant : Equipment
{
    
    public override bool CanUse() 
    {
        bool canUse = base.CanUse();
        bool onBounds = PositionComponent.OnMap(transform.position);
        // bool onRoad = RoadConfigComponent.OnRoad((int)transform.position.x, (int)transform.position.z);

        return canUse && SeedsComponent.LocalCount > 0 && onBounds && CursorMUD.Entity == null;

    }

    
    public override async UniTask<bool> Use() {
        
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        //can try optimistic

        return await ActionsMUD.ActionTx(ourComponent.Entity, ActionName.Plant, new Vector3(x, 0, y));
    }

}
