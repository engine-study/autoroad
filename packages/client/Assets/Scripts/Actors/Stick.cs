using UnityEngine;
using mudworld;

public class Stick : Equipment
{

    public override bool IsInteractable() {

        bool canUse = base.IsInteractable();
        if(entity == null) return false;
        
        NPCComponent npc = entity.GetMUDComponent<NPCComponent>();
        MoveComponent moveType = entity.GetMUDComponent<MoveComponent>();

        Vector3 pushToPos = transform.position + (transform.position - Sender.transform.position).normalized;

        bool onBounds = PositionComponent.OnWorldOrMap(entity, pushToPos);
        return canUse && onBounds && npc != null && moveType != null && moveType.MoveType == MoveType.Push; 
    }
    
}
