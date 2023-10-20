using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class Melee : Equipment
{

    public override bool IsInteractable() {
        bool canUse = base.IsInteractable();

        mud.MUDEntity e = CursorMUD.Entity;
        if(e == null) return false;
        
        NPCComponent npc = e?.GetMUDComponent<NPCComponent>();
        bool onBounds = PositionComponent.OnMap(transform.position);
        return canUse && onBounds && npc != null; 
    }
    
    public override async UniTask<bool> Use() {
        
        Debug.Log("MELEEING");
        Vector3 melee = transform.position;
        return await ActionsMUD.ActionTx(Entity, ActionName.Melee, melee);
    }
    
}
