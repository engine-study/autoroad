using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;

public class Plant : Equipment
{
    
    public override bool IsInteractable() 
    {
        canUse = base.IsInteractable() && (entityUnder == null || entityUnder.GetMUDComponent<RoadComponent>()?.State == RoadState.Path);
        return canUse;

    }

}
