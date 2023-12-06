using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
public enum PaymentType {None, Coins, Gems, Eth}
public enum ItemType { GameplayStashable, GameplayEquipment, Cosmetic, PayedCosmetic }

[CreateAssetMenu(fileName = "Item", menuName = "Gaul/Item", order = 1)]
public class GaulItem : ScriptableObject, IComparable<GaulItem> {

    public string Name {get{return itemName;}}
    public bool InMileRange {get{return mileRange == Vector2.zero || (GameStateComponent.MILE_COUNT >= mileRange.x && GameStateComponent.MILE_COUNT <= mileRange.y);}}
    public bool HighEnoughLevel {get{return XPComponent.LocalLevel >= minLevel;}}
    

    [Header("Item")]
    [SerializeField] string itemName = "Item";
    [TextArea(1,5)] public string itemDescription = "";
    public Sprite itemSprite;

    [Header("Info")]
    public ItemType itemType;
    public PlayerBody bodyPart;
    public int ID = -1;
    
    [Header("Store")]
    public bool canBuy = false;
    public bool isRare;
    public Vector2 mileRange = Vector2.zero;
    public int minLevel = 0;
    public int Multiplier = 1;
    [SerializeField] Currency value;


    public string FullDescription() {
        return "<b><size=32>" + itemName + "</size></b> - <size=28>" + ItemTypeString(itemType) + "</size>\n" + itemDescription;
    }

    public static string ItemTypeString(ItemType newType) { return ItemStrings[(int)newType]; }

    public static string [] ItemStrings = new string[]{"Item", "Tool", "Outfit", "Special"};


    public int CompareTo(GaulItem other) {
        // First compare by coins (price)
        int priceComparison = value.DefaultValue.CompareTo(other.value.DefaultValue);
        return priceComparison;
        
    }
    
    public float StatToValue(StatType statType) { 

        if(statType == StatType.RoadCoin && Inventory.LocalInventory && Inventory.LocalInventory.ItemUnlocked(this) && Multiplier > 1) {
            return Inventory.LocalInventory.GetItemSlot(this).amount + Inventory.LocalInventory.GetItemSlot(this).amount * value.StatToValue(statType);
        } else {
            return value.StatToValue(statType);
        }
    }  

}

[System.Serializable]
public class Currency {
    [SerializeField] float price = 0;
    [SerializeField] int gem = 0;
    [SerializeField] float eth = 0f;
    [SerializeField] float XP = 0f;

    public float DefaultValue {get{
        if(price > 0) return price;
        if(gem > 0) return gem;
        if(eth > 0) return eth;
        Debug.LogError("Free item");
        return 0f;
    }}

    public float StatToValue(StatType statType) {

        if(statType == StatType.RoadCoin) {
            return price;
        } else if(statType == StatType.Gem) {
            return gem;
        } else if(statType == StatType.Eth) {
            return eth;
        } else if(statType == StatType.XP) {
            return XP;
        } else return -1;

    }

    //TODO add CompareTo
}