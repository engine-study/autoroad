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
        RockComponent rock = CursorMUD.Entity?.GetMUDComponent<RockComponent>();
        return canUse && CursorMUD.Entity != null && rock != null && rock.RockType < RockType.Nucleus;
    }

    public override async UniTask<bool> Use() {
    
        // RockComponent rock = CursorMUD.Entity.GetMUDComponent<RockComponent>();
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, ActionName.Mining, transform.position);
    }

}
 