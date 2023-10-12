using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using System;

public class StatsList : EntityUI
{
    [Header("Debug")]
    [SerializeField] ValueComponent component;

    [Header("Stats")]
    [SerializeField] StatUI statPrefab;

    [EnumNamedArray( typeof(StatType) )]
    [SerializeField] StatUI [] stats;

    public override void Init() {
        if(hasInit) {return;}

        Init();
        statPrefab.ToggleWindowClose();
        stats = new StatUI[(int)StatType._Count];
    }

    public override void UpdateEntity() {
        base.UpdateEntity();

        for(int i = 0; i < stats.Length; i++) {

            component = (ValueComponent)Entity.GetMUDComponent(StatUI.StatToComponent((StatType)i));

            if(component) {
                if(stats[i] == null) { stats[i] = Instantiate(statPrefab, rect);}
                stats[i].SetValue((StatType)i, component.String);
            } else {
                
            }

            if(stats[i]) stats[i].ToggleWindow(component != null);
        }

        UpdateComponent();
    }

    public virtual void UpdateComponent() {
        
    }

}
