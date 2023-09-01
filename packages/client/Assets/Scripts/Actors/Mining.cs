using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud.Unity;

public class Mining : Equipment
{
    public override bool CanUse() {
        bool canUse = base.CanUse();
        return false && canUse && CursorMUD.MUDEntity != null && CursorMUD.MUDEntity.GetMUDComponent<RockComponent>(); //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
    }

    public override async UniTask<bool> Use() {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        Debug.Log("Shoveled: " + x + "," + y);
        
        string entity = MUDHelper.Keccak256("Road", x,y);
        
        List<TxUpdate> updates = new List<TxUpdate>();
        return await ActionsMUD.DoAction(updates, ActionName.Mining, new Vector3(x, 0, y));
    }

}
 