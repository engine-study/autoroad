using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;
using IWorld.ContractDefinition;
public abstract class ValueComponent : MUDComponent {

    public float Value {get{return value;}}
    public string String {get{return valueString;}}
    public StatType Type {get{return type;}}
    
    [Header("Value")]
    [SerializeField] float value;
    [SerializeField] string valueString;
    [SerializeField] StatType type;
    
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        value = SetValue(update);
        valueString = SetString(update);
    }

    protected abstract float SetValue(IMudTable update);
    protected virtual string SetString(IMudTable update) {return Value.ToString("00");}
    protected abstract StatType SetStat(IMudTable update);

}
