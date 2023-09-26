using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PaymentType {None, Coins, Gems, Eth}

[CreateAssetMenu(fileName = "Item", menuName = "Gaul/Item", order = 1)]
public class GaulItem : ScriptableObject {

    public string itemName = "Item";
    public int ID = -1;

    [TextArea(1,5)]
    public string itemDescription = "";
    public ItemType itemType;
    public PaymentType paymentType;
    public bool isRare;
    public int minLevel = 0;
    public float price = 0;
    public int gem = 0;
    public float eth = 0f;

    public string FullDescription() {
        return "<b><size=32>" + itemName + "</size></b> - <size=28>" + ItemTypeString(itemType) + "</size>\n" + itemDescription;
    }

    public string ItemTypeString(ItemType newType) { return ItemStrings[(int)newType]; }

    public static string [] ItemStrings = new string[]{"Item", "Tool", "Outfit", "Unique Outfit"};
}

public enum ItemType { GameplayStashable, GameplayEquipment, Cosmetic, PayedCosmetic }