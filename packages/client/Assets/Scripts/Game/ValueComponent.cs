using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
public abstract class ValueComponent : MUDComponent {

    public List<InventorySlot> Items {get{return slots;}}
    public float Value {get{return value;}}
    public string String {get{return valueString;}}
    public StatType StatType {get{return statType;}}
    
    [Header("Value")]
    [SerializeField] GaulItem item;
    // [SerializeField] GaulItem [] items;

    [Header("Debug")]
    [SerializeField] float value;
    [SerializeField] string valueString;
    [SerializeField] StatType statType;
    List<InventorySlot> slots;

    protected abstract float SetValue(IMudTable update);
    protected abstract StatType SetStat(IMudTable update);
    protected virtual string SetString(IMudTable update) {return Value.ToString("00");}


    protected override void Init(SpawnInfo newSpawnInfo)
    {
        base.Init(newSpawnInfo);
        if(slots  == null) {slots = new List<InventorySlot>();}
        slots.Add(new InventorySlot(){item = item});
        
        // for(int i = 0; i < items.Length; i++) {
        //     slots.Add(new InventorySlot(){item = items[i]});
        // }
    }
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        value = SetValue(update);
        valueString = SetString(update);
        statType = SetStat(update);

        //simple inventory
        slots[0].amount = (int)value;
        
    }



}
