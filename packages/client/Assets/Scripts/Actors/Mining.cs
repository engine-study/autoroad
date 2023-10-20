using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud;

public class Mining : Equipment
{
    public override bool IsInteractable() {
        bool canUse = base.IsInteractable();
        RockComponent rock = CursorMUD.Entity?.GetMUDComponent<RockComponent>();
        return canUse && CursorMUD.Entity != null && rock != null && rock.RockType < RockType.Nucleus;
    }

    public override async UniTask<bool> Use() {
    
        // RockComponent rock = CursorMUD.Entity.GetMUDComponent<RockComponent>();
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, ActionName.Mining, transform.position);
    }

}
 