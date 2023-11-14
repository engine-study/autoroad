using UnityEngine;
using mudworld;
using mud;

public class ConscriptionComponent : ValueComponent
{

    protected override float SetValue(MUDTable update) {return (bool)((ConscriptionTable)update).Value ? 1 : 0;}
    protected override StatType SetStat(MUDTable update) {return StatType.None;}
    protected override MUDTable GetTable() {return new ConscriptionTable();}

}
