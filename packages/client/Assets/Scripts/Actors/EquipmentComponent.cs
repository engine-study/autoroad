using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using mud.Unity;
using mud.Client;
using System;

public class EquipmentComponent : Equipment
{
    [Header("MUD Equipment")]
    public ActionName action;
    public MUDComponent prefab;
    MUDEntity entity;
    // public List<MUDComponent> prefab;

    protected override void Awake() {
        base.Awake();
        entity = GetComponentInParent<MUDEntity>();
    }

    public override bool CanUse() {
        bool canUse = base.CanUse();
        bool hasExpectedPrefab = (prefab == null && CursorMUD.Entity == null) || (CursorMUD.Entity != null && CursorMUD.Entity.ExpectedComponents.Contains(prefab.GetType()));
        return false && canUse && hasExpectedPrefab; 
    }

    public override async UniTask<bool> Use() {

        int x = (int)CursorMUD.GridPos.x;
        int y = (int)CursorMUD.GridPos.z;

        Debug.Log(action.ToString() + ": " + x + "," + y);
                
        return await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, action, new Vector3(x, 0, y));
    }

}
 