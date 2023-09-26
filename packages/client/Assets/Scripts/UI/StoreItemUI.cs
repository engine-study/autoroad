using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : SPWindow
{
    [Header("Store Item")]
    [HideInInspector] public StoreUI store;
    [SerializeField] SPInputField itemText;
    [SerializeField] SPButton buyButtonCoin;
    [SerializeField] SPButton buyButtonGem;
    [SerializeField] SPButton buyButtonEth;
    [SerializeField] Image itemTypeImage;
    [SerializeField] Sprite [] itemTypeSprites;

    [Header("Debug")]
    [SerializeField] GaulItem item;

    public void SetItem(GaulItem newItem) {

        item = newItem;
        itemText.UpdateField(item.itemName);

        buyButtonCoin.UpdateField(item.price.ToString("000"));
        buyButtonGem.UpdateField(item.gem.ToString("0"));
        buyButtonEth.UpdateField(item.eth.ToString(".00"));

        buyButtonCoin.ToggleWindow(item.price > 0);
        buyButtonGem.ToggleWindow(item.gem > 0);
        buyButtonEth.ToggleWindow(item.eth > 0);
        
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
        ToggleWindow(XPComponent.LocalLevel >= item.minLevel);
        buyButtonCoin.ToggleState(CoinComponent.LocalCoins >= item.price ? SPSelectableState.Default : SPSelectableState.Disabled);
        buyButtonGem.ToggleState(GemComponent.LocalGems >= item.gem ? SPSelectableState.Default : SPSelectableState.Disabled);
        buyButtonEth.ToggleState(CoinComponent.LocalCoins >= item.price ? SPSelectableState.Default : SPSelectableState.Disabled);
    }
}
