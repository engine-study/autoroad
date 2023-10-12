using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mud.Client;

public enum StatType {None, RoadCoin, Gem, Eth, XP, Level, Scroll, Seed, Strength, Weight, Health, _Count}
public class StatUI : EntityUI
{
    public SPButton Button{get{return button;}}
    public bool IsValue {get{return type >= StatType.RoadCoin && type <= StatType.XP;}}
    public bool IsWeight {get{return type >= StatType.Strength && type <= StatType.Weight;}}

    [Header("Stat")]
    [SerializeField] StatType type;
    [SerializeField] SPButton button;

    [Header("Graphics")]
    [SerializeField] bool useColors = false;

    [EnumNamedArray( typeof(StatType) )]
    public Sprite [] Sprites;

    [EnumNamedArray( typeof(StatType) )]
    public Color [] Colors;
    public Color RewardColor;
    public Color PenaltyColor;

    public override void Init() {
        if(hasInit) return;
        base.Init();

    }

    public static Type StatToComponent(StatType statType) {
        if(statType == StatType.RoadCoin) { return typeof(CoinComponent);
        } else if(statType == StatType.Gem) { return typeof(GemComponent);
        } else if(statType == StatType.Eth) { return typeof(CoinComponent);
        } else if(statType == StatType.XP) { return typeof(XPComponent);
        } else if(statType == StatType.Level) { return typeof(XPComponent);
        } else if(statType == StatType.Scroll) { return typeof(ScrollComponent);
        } else if(statType == StatType.Seed) { return typeof(SeedComponent);
        } else if(statType == StatType.Strength) { return typeof(WeightComponent);
        } else if(statType == StatType.Weight) { return typeof(WeightComponent);
        } else if(statType == StatType.Health) { return typeof(HealthComponent);
        } else { Debug.LogError("Unhandled"); return null;}

    }

    public void SetValue(string newStat) {
        SetValue(type, newStat);
    }

    public void SetValue(StatType newType, string newStat) {

        if((int)newType >= Sprites.Length || Sprites[(int)newType] == null) {Debug.LogError(newType + " not available.", this); return;}

        type = newType;

        button.UpdateField(newStat);
        button.Image.sprite = Sprites[(int)newType];


    }

}
