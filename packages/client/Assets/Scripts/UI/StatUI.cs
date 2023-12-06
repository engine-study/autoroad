using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using mud;
using UnityEngine.UI;

public enum StatType {None, RoadCoin, Gem, Eth, XP, Level, Scroll, Seed, Strength, Weight, Health, ScrollSwap, Grapeleaf, _Count}
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
        } else { Debug.LogError(statType + " Unhandled"); return null;}

    }

    public static string StatToString(StatType statType, float value) {
        if(statType == StatType.RoadCoin) { return FormatNumber((int)value);
        } else if(statType == StatType.Eth) { return value.ToString("##0.00");
        } else if(statType == StatType.Level) { return value.ToString("#0");
        } else { return ((int)value).ToString("#0");}

    }

    public void SetValue(string newStat) {
        SetValue(type, newStat);
    }

    public void SetValue(int newStat) {
        SetValue(type, (float)newStat);
    }

    public void SetValue(StatType newType, float newStat) {
        SetValue(newType, StatToString(newType, newStat));
    }

    
    static string FormatNumber(int num) {
        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";

        if (num >= 10000)
            return (num / 1000D).ToString("0") + "K";

        if (num >= 1000)
            return (num / 100D).ToString("0") + "K";

        return num.ToString("#,0");
    }

    public void SetValue(StatType newType, string newStat) {

        if(!hasInit) {Init();}

        if((int)newType >= Sprites.Length || Sprites[(int)newType] == null) {Debug.LogError(newType + " not available.", this); return;}
        if((int)newType >= Colors.Length || Colors[(int)newType] == null) {Debug.LogError(newType + " not available.", this); return;}

        type = newType;

        button.UpdateField(newStat);
        button.Image.sprite = Sprites[(int)newType];

        if(useColors) {
            
            if(IsValue) {
                bgColor = float.Parse(newStat) >= 0f ? RewardColor : PenaltyColor;
            } else if(IsWeight) {
                if(Colors[(int)newType].a > 0f) {bgColor = Colors[(int)newType];}
            } else {
                if(Colors[(int)newType].a > 0f) {bgColor = Colors[(int)newType];}
            }

            ApplyGraphics();

        }
    }

}
