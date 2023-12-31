using UnityEngine;
using mudworld;

public class FishingRod : Equipment {
    
    public override bool IsInteractable() {
        bool canUse = base.IsInteractable();

        if(entity == null) return false;
        
        MoveComponent moveType = entity.GetMUDComponent<MoveComponent>();
        WeightComponent weight = entity.GetMUDComponent<WeightComponent>();

        Vector3 pushObject = transform.position;
        Vector3 pushToPos = Sender.transform.position - (transform.position - Sender.transform.position).normalized;

        if(!PositionComponent.OnWorldOrMap(entity, pushToPos)) {return false;}

        return canUse && moveType != null && moveType.MoveType == MoveType.Push && (weight == null || weight.Weight + WeightComponent.LocalWeight <= 0);
    }
}
