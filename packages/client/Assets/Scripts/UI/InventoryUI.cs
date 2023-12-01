using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
public class InventoryUI : SPWindowViews
{
    [Header("Inventory")]
    [EnumNamedArray( typeof(CosmeticType) )]
    [SerializeField] GameObject[] headers;

    [SerializeField] SPButton[] buttons;

}
