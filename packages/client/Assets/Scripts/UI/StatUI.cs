using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType {None, RoadCoin, Gem, Eth, XP, Level, Scroll, Seed, Strength, Weight}
public class StatUI : SPWindowEntity
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

    public void SetValue(string newStat) {
        SetValue(type, newStat);
    }

    public void SetValue(StatType newType, string newStat) {

        if((int)newType >= Sprites.Length || Sprites[(int)newType] == null) {Debug.LogError(newType + " not available.", this); return;}


        type = newType;

        if(newType == StatType.RoadCoin) {

        } else if(newType == StatType.Gem) {

        } else if(newType == StatType.Eth) {

        } else if(newType == StatType.XP) {

        } else if(newType == StatType.Level) {

        } else if(newType == StatType.Scroll) {

        } else if(newType == StatType.Seed) {

        } else {
            Debug.LogError("Unhandled", this);
        }

        button.UpdateField(newStat);
        button.Image.sprite = Sprites[(int)newType];


    }

}
