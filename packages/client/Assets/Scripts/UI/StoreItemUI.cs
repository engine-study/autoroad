using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemUI : SPWindow
{

    public GaulItem Item {get{return item;}}

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

        buyButtonCoin.SetValue( StatType.RoadCoin, item.StatToValue(StatType.RoadCoin));
        buyButtonGem.SetValue( StatType.Gem, item.StatToValue(StatType.Gem));
        buyButtonEth.SetValue( StatType.Eth, item.StatToValue(StatType.Eth));

        buyButtonCoin.ToggleWindow(item.StatToValue(StatType.RoadCoin) > 0);
        buyButtonGem.ToggleWindow(item.StatToValue(StatType.Gem) > 0);
        buyButtonEth.ToggleWindow(item.StatToValue(StatType.Eth) > 0);
        
        itemTypeImage.sprite = itemTypeSprites[(int)item.itemType];

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

        SetItem(item);

        bool isValid = (item.HighEnoughLevel && item.InMileRange) || SPGlobal.IsDebug;
        bool doesNotOwn = item.itemType == ItemType.GameplayStashable || Inventory.LocalInventory.ItemUnlocked(item) == false;
        
        ToggleWindow(isValid && doesNotOwn);

        if(!isValid || !doesNotOwn) {return;}

        buyButtonCoin.Button.ToggleState(CoinComponent.LocalCoins >= item.StatToValue(StatType.RoadCoin) ? SPSelectableState.Default : SPSelectableState.Disabled);
        buyButtonGem.Button.ToggleState(GemComponent.LocalGems >= item.StatToValue(StatType.Gem) ? SPSelectableState.Default : SPSelectableState.Disabled);

        //todo get eth value of player
        buyButtonEth.Button.ToggleState(SPSelectableState.Default);
        //CoinComponent.LocalCoins >= item.StatToValue(StatType.Eth)
    }
}
