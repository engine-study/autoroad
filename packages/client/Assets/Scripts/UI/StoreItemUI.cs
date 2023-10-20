using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : SPWindow
{
    [Header("Store Item")]
    [HideInInspector] public StoreUI store;
    [SerializeField] SPButton itemText;
    [SerializeField] StatUI buyButtonCoin;
    [SerializeField] StatUI buyButtonGem;
    [SerializeField] StatUI buyButtonEth;
    [SerializeField] Image itemTypeImage;
    [SerializeField] Sprite [] itemTypeSprites;

    [Header("Debug")]
    [SerializeField] GaulItem item;

    public void SetItem(GaulItem newItem) {

        item = newItem;
        itemText.UpdateField(item.Name);

        buyButtonCoin.SetValue( StatType.RoadCoin, item.value.price);
        buyButtonGem.SetValue( StatType.Gem, item.value.gem);
        buyButtonEth.SetValue( StatType.Eth, item.value.eth);

        buyButtonCoin.ToggleWindow(item.value.price > 0);
        buyButtonGem.ToggleWindow(item.value.gem > 0);
        buyButtonEth.ToggleWindow(item.value.eth > 0);
        
        itemTypeImage.sprite = itemTypeSprites[(int)item.itemType];

        CanBuy();
    }

    public void UpdateDescription() {
        SPRawText text = SPHoverWindow.Instance.GetComponentInChildren<SPRawText>(true);
        text.UpdateField(item.FullDescription());
    }

    public void BuyCoins() {
        store.Buy(item, PaymentType.Coins);
    }

    public void BuyGems() {
        store.Buy(item, PaymentType.Gems);
    }

    public void BuyEth() {
        store.Buy(item, PaymentType.Eth);
    }

    public void CanBuy() {
        bool display = item.HighEnoughLevel && item.InMileRange;
        ToggleWindow(display);

        if(!display) {return;}

        buyButtonCoin.Button.ToggleState(CoinComponent.LocalCoins >= item.value.price ? SPSelectableState.Default : SPSelectableState.Disabled);
        buyButtonGem.Button.ToggleState(GemComponent.LocalGems >= item.value.gem ? SPSelectableState.Default : SPSelectableState.Disabled);
        buyButtonEth.Button.ToggleState(CoinComponent.LocalCoins >= item.value.eth ? SPSelectableState.Default : SPSelectableState.Disabled);
    }
}
