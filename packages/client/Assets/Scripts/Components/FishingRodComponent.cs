using UnityEngine;
using mudworld;
using mud;

public class FishingRodComponent : PropComponent {
 
    protected override MUDTable GetTable() {return new FishingRodTable();}
    protected override StatType SetStat(MUDTable mudTable) {return StatType.None;}

}
