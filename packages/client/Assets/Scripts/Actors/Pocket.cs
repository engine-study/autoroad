using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mudworld;

public class Pocket : Equipment
{
    public override bool IsInteractable() {
        CarryTable ct = MUDTable.GetTable<CarryTable>(NetworkManager.LocalKey);
        bool isPickingUp = ct == null || ct.Value == "0x";
        canUse = base.IsInteractable() && ((isPickingUp && entity && entity.GetMUDComponent<PlayerComponent>() == null) || (!entity));
        return canUse;
    }
}
