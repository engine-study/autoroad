using UnityEngine;
using DefaultNamespace;
using mud.Client;

public class RobeComponent : MUDComponent {


    protected override IMudTable GetTable() {return new RobeTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        RobeTable table = update as RobeTable;

    }

}
