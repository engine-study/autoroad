using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItemUI : SPWindow
{
    [HideInInspector] public StoreUI store;
    [SerializeField] GaulItem item;
    [SerializeField] SPInputField itemText;
    [SerializeField] SPButton buyButton;

    public void SetItem(GaulItem newItem) {

        item = newItem;
        itemText.UpdateField(item.itemName);
        buyButton.UpdateField(item.price.ToString("000"));
        CanBuy();
    }

    public void Buy() {
        store.Buy(item);
    }

    public void CanBuy() {
        ToggleWindow(XPComponent.LocalLevel >= item.minLevel);
        buyButton.ToggleState(CoinComponent.LocalCoins >= item.price ? SPSelectableState.Default : SPSelectableState.Disabled);
    }
}