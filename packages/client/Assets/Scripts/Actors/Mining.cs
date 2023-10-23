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
        canUse = base.IsInteractable() && entity?.GetMUDComponent<RockComponent>()?.RockType < RockType.Nucleus;
        return canUse;
    }

}
 