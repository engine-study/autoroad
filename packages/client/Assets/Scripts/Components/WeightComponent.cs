using UnityEngine;
using mudworld;
using mud;
using mud;
using System;

public class WeightComponent : ValueComponent {

    public static int LocalWeight;
    public int Weight {get { return weight; } }
    
    [Header("Weight")]
    [SerializeField] private int weight;

    protected override float SetValue(MUDTable update) {return Mathf.Abs((int)((WeightTable)update).Value);}
    protected override StatType SetStat(MUDTable update) {if((int)((WeightTable)update).Value < 0) return StatType.Strength; else return StatType.Weight;}
    
    protected override MUDTable GetTable() {return new WeightTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {
        base.UpdateComponent(update,newInfo);

        WeightTable table = update as WeightTable;
        weight = (int)table.Value;

        if(Entity.Key == NetworkManager.LocalKey) {
            LocalWeight = weight;
        }
    }


}
