using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class ActionComponent : MUDComponent {

    public static ActionComponent LocalState;
    public ActionName Action {get { return actionType; } }
    public Vector3 Position {get { return targetPos; } }
    public PositionSync Target {get { return targetSync; } }

    [Header("State")]
    [SerializeField] ActionName actionType;
    [SerializeField] Vector3 targetPos;
    [SerializeField] PositionSync targetSync;

    [Header("Target")]
    [SerializeField] string targetBytes;
    [SerializeField] MUDEntity targetEntity;


    protected override void PostInit() {
        base.PostInit();


        if (Entity.Key == NetworkManager.LocalAddress) {
            LocalState = this;
        }
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        LocalState = null;
    }


    protected override IMudTable GetTable() {return new ActionTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        ActionTable table = update as ActionTable;

        actionType = (ActionName)table.action;
        targetPos = new Vector3((int)table.x, 0f, (int)table.y);

        targetBytes = ((string)table.target).ToLower();
        targetEntity = MUDWorld.FindEntity(targetBytes);

        if(targetEntity) {
            targetSync = targetEntity.GetRootComponent<PositionSync>();
            if(targetSync == null) {Debug.LogError("No pos for action", this);return;}
        }

    }




}
