using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class StateComponent : MUDComponent {

    public StateType State {get { return stateType; } }

    [Header("State")]
    [SerializeField] private StateType stateType;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private PlayerComponent player;
    [SerializeField] private ActionsMUD actions;


    protected override void PostInit() {
        base.PostInit();

        player = Entity.GetMUDComponent<PlayerComponent>();
        actions = player.PlayerScript.Actions;
    }
    

    protected override IMudTable GetTable() {return new StateTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        StateTable table = update as StateTable;

        stateType = (StateType)table.state;
        targetPos = new Vector3((int)table.x, 0f, (int)table.y);

        if(player == null || player.IsLocalPlayer) {
            return;
        }

        actions.StateToAction(stateType, targetPos);

    }



}
