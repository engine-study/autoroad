using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
public abstract class ValueComponent : MUDComponent {

    public GaulItem Item {get{return item;}}
    public float Value {get{return value;}}
    public string String {get{return valueString;}}
    public StatType StatType {get{return statType;}}
    
    [Header("Value")]
    [SerializeField] GaulItem item;

    [Header("Debug")]
    [SerializeField] float value;
    [SerializeField] string valueString;
    [SerializeField] StatType statType;
    protected abstract float SetValue(IMudTable update);
    protected abstract StatType SetStat(IMudTable update);
    protected virtual string SetString(IMudTable update) {return Value.ToString("00");}

    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        value = SetValue(update);
        valueString = SetString(update);
        statType = SetStat(update);
        
    }



}
