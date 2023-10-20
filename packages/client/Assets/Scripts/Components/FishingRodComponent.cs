using UnityEngine;
using mudworld;
using mud;

public class FishingRodComponent : PropComponent {
 
    protected override IMudTable GetTable() {return new FishingRodTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        FishingRodTable table = update as FishingRodTable;
    }

    protected override StatType SetStat(IMudTable mudTable) {return StatType.None;}

}
