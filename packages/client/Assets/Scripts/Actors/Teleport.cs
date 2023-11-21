using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;

public class Teleport : Equipment
{
    public override bool IsInteractable() {

        if(actionName == ActionName.Teleport) {
            return base.IsInteractable(); 
        } else if(actionName == ActionName.Swap) {
            
            bool IsPerpendicular = PositionComponent.IsPerpendicular(transform.position, SPPlayer.LocalPlayer.transform.position);
            MUDEntity e = IsPerpendicular ? PositionComponent.GetFirstObjectAlong(transform.position, (SPPlayer.LocalPlayer.transform.position - transform.position).normalized) : null;
            MoveComponent m = e?.GetMUDComponent<MoveComponent>();
            return IsPerpendicular && e != null && m?.MoveType != MoveType.Permanent && base.IsInteractable();
            
        } else {
            return false;
        }

    }
}
