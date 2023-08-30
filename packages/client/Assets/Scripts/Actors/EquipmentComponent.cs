using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud.Unity;
using System;

public class EquipmentComponent : Equipment
{
    [Header("MUD Equipment")]
    public StateType action;
    public MUDComponent prefab;
    // public List<MUDComponent> prefab;
    
    public override bool CanUse() {
        bool canUse = base.CanUse();
        bool hasExpectedPrefab = (prefab == null && CursorMUD.MUDEntity == null) || (CursorMUD.MUDEntity != null && CursorMUD.MUDEntity.ExpectedComponents.Contains(prefab));
        return false && canUse && hasExpectedPrefab; 
    }

    public override async UniTask<bool> Use() {
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        Debug.Log(action.ToString() + ": " + x + "," + y);
                
        List<TxUpdate> updates = new List<TxUpdate>();
        return await ActionsMUD.DoAction(updates, action, new Vector3(x, 0, y));
    }

}
 