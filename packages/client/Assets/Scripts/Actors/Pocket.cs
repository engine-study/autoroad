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
        bool isPickingUp = ct == null || string.IsNullOrEmpty(ct.Value.Replace("0", "").Replace("x", ""));
        bool canPickUpOrPutDown = (isPickingUp && entity && entity.GetMUDComponent<PlayerComponent>() == null) || (!isPickingUp && !entity);
        // Debug.Log("base: " + base.IsInteractable());
        // Debug.Log("canPickUpOrPutDown: " + canPickUpOrPutDown);
        canUse = base.IsInteractable() && canPickUpOrPutDown;
        return canUse;
    }
}
