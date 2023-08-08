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
        bool onBounds = BoundsComponent.OnBounds((int)transform.position.x, (int)transform.position.z);

        return canUse && SeedsComponent.LocalCount > 0 && onBounds && CursorMUD.MUDEntity == null;

    }

    
    public override async UniTask<bool> Use() {
        
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        //can try optimistic

        return await TxManager.Send<PlantFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y));
    }

}
