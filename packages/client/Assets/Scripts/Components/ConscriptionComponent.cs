using UnityEngine;
using mudworld;
using mud;

public class ConscriptionComponent : ValueComponent
{

    protected override float SetValue(IMudTable update) {return (bool)((ConscriptionTable)update).Value ? 1 : 0;}
    protected override StatType SetStat(IMudTable update) {return StatType.None;}
    protected override IMudTable GetTable() {return new ConscriptionTable();}

}
