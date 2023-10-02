using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;


public class Mining : Equipment
{
    public override bool CanUse() {
        bool canUse = base.CanUse();
        RockComponent rock = CursorMUD.MUDEntity.GetMUDComponent<RockComponent>();
        return false && canUse && CursorMUD.MUDEntity != null && rock != null && rock.RockType < RockType.Nucleus; //distanceToPlayer > .5f && distanceToPlayer <= 1f && 
    }

    public override async UniTask<bool> Use() {
        return await new UniTask<bool>();
        // int x = (int)transform.position.x;
        // int y = (int)transform.position.z;

        // Debug.Log("Shoveled: " + x + "," + y);

        // string entity = MUDHelper.Keccak256("Road", x,y);

        // List<TxUpdate> updates = new List<TxUpdate>();
        // return await ActionsMUD.ActionTx(ourComponent.Entity, ActionName.Mining, new Vector3(x, 0, y));
    }

}
 