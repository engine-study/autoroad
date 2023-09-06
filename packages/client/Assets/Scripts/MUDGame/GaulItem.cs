using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Gaul/Item", order = 1)]
public class GaulItem : ScriptableObject
{

    public string itemName = "Item";
    [TextArea(1,5)]
    public string itemDescription = "";
    public ItemType itemType;
    public bool isRare;
    public int minLevel = 0;
    public int price = 0;
}

public enum ItemType { GameplayStashable, GameplayEquipment, Cosmetic, PayedCosmetic }