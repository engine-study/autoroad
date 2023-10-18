using UnityEngine;
using mudworld;
using mud;
using mud;
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

        if (Entity.Key == NetworkManager.LocalKey) {
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

        actionType = (ActionName)table.Action;
        targetPos = new Vector3((int)table.X, 0f, (int)table.Y);

        targetBytes = ((string)table.Target).ToLower();
        targetEntity = MUDWorld.FindEntity(targetBytes);

        if(Loaded) {
            if(targetEntity) {
                targetSync = targetEntity.GetRootComponent<PositionSync>();
                if(targetSync == null) {Debug.LogError("No pos for action", this);return;}
            } else {
                targetSync = null;
            }
        }

    }




}
