using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class Shovel : Equipment
{
    public override bool IsInteractable() {
        canUse = base.IsInteractable() && RoadConfigComponent.OnRoad(transform.position) && entityUnder != null && entityUnder.GetMUDComponent<RoadComponent>()?.State == RoadState.Path; //distanceToPlayer > .5f && distanceToPlayer <= 1f &&
        return canUse;
    }

}
