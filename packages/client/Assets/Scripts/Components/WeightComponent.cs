using UnityEngine;
using DefaultNamespace;
using mud;

using System;

public class WeightComponent : MUDComponent {

 
    public int Weight {get { return weight; } }
    
    [Header("Weight")]
    [SerializeField] private int weight;

    protected override IMudTable GetTable() {return new WeightTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        WeightTable table = update as WeightTable;
        weight = (int)table.value;
    }


}
