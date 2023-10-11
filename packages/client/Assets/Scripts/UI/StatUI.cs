using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType {None, RoadCoin, Gem, Eth, XP, Level, Scroll, Seed, Strength, Weight}
public class StatUI : SPWindowEntity
{
    public SPButton Button{get{return button;}}

    [Header("Stat")]
    [SerializeField] ResourceType type;
    [SerializeField] SPButton button;

    [Header("Graphics")]
    [SerializeField] bool useColors = false;

    [EnumNamedArray( typeof(ResourceType) )]
    public Sprite [] Sprites;
    public Color [] Colors;
    public Sprite Weight, Strength;


    public override void Init() {
        if(hasInit) return;
        base.Init();

    }

    public void SetValue(string newStat) {
        SetValue(type, newStat);
    }

    public void SetValue(ResourceType newType, string newStat) {

        if((int)newType >= Sprites.Length || Sprites[(int)newType] == null) {Debug.LogError(newType + " not available.", this); return;}


        type = newType;

        if(newType == ResourceType.RoadCoin) {

        } else if(newType == ResourceType.Gem) {

        } else if(newType == ResourceType.Eth) {

        } else if(newType == ResourceType.XP) {

        } else if(newType == ResourceType.Level) {

        } else if(newType == ResourceType.Scroll) {

        } else if(newType == ResourceType.Seed) {

        } else {
            Debug.LogError("Unhandled", this);
        }

        button.UpdateField(newStat);
        button.Image.sprite = Sprites[(int)newType];


    }

}
