using UnityEngine;
using DefaultNamespace;
using mud;

public class FishingRodComponent : PropComponent {
 
    protected override IMudTable GetTable() {return new FishingRodTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        FishingRodTable table = update as FishingRodTable;
    }

}
