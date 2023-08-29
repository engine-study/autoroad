using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public class StateComponent : MUDComponent {

    public StateType State {get { return stateType; } }

    [Header("State")]
    [SerializeField] private StateType stateType;

    protected override IMudTable GetTable() {return new StateTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        StateTable table = update as StateTable;
        stateType = (StateType)table.state;
    }

}
