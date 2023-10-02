using UnityEngine;
using DefaultNamespace;
using mud;

using System;

public class ActionComponent : MUDComponent {

    public static ActionComponent LocalState;
    public ActionName Action {get { return actionType; } }
    public Vector3 Position {get { return targetPos; } }

    [Header("State")]
    [SerializeField] private ActionName actionType;
    [SerializeField] private Vector3 targetPos;


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

        actionType = (ActionName)table.action;
        targetPos = new Vector3((int)table.x, 0f, (int)table.y);

    }




}
