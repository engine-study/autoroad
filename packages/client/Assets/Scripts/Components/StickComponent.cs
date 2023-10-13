using UnityEngine;
using mudworld;
using mud;

public class StickComponent : PropComponent {

    protected override IMudTable GetTable() {return new StickTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        StickTable table = update as StickTable;
    }

}
