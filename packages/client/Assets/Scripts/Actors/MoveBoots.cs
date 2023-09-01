using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;
public class MoveBoots : Equipment
{
    
    public override bool CanUse() 
    {
        bool canUse = base.CanUse();

        return canUse;

    }

    
    public override async UniTask<bool> Use() {
        
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        //can try optimistic

        return await ActionsMUD.ActionTx(null, ActionName.Walking, new Vector3(x,0,y));

    }

}