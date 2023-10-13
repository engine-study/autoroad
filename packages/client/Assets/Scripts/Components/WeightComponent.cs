using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class WeightComponent : ValueComponent {

 
    public int Weight {get { return weight; } }
    
    [Header("Weight")]
    [SerializeField] private int weight;

    protected override float SetValue(IMudTable update) {return Mathf.Abs((int)((WeightTable)update).value);}
    protected override StatType SetStat(IMudTable update) {if((int)((WeightTable)update).value < 0) return StatType.Strength; else return StatType.Weight;}
    
    protected override IMudTable GetTable() {return new WeightTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        WeightTable table = update as WeightTable;
        weight = (int)table.value;
    }


}
