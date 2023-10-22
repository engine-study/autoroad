using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWorld.ContractDefinition;
using mud;

public class StoreUI : SPWindowParent
{

    [Header("Store")]
    public SPButton notification;
    public StoreItemUI itemPrefab;
    public RectTransform itemRect;


    [EnumNamedArray( typeof(ItemType) )]
    public SPButton [] itemTypeHeaders;
    public AudioClip [] sfx_buy;
    int newItemCount;

    [Header("Debug")]
    public List<GaulItem> itemInfo = new List<GaulItem>();
    public List<StoreItemUI> items = new List<StoreItemUI>();
    

    protected override void Start() {
        base.Start();

        itemPrefab.ToggleWindowClose();

        for(int i =0; i < itemTypeHeaders.Length; i++) {
            itemTypeHeaders[i].UpdateField(GaulItem.ItemTypeString((ItemType)i));
        }

        object[] itemObjects = Resources.LoadAll("Data/Store");

        for (int i = 0; i < itemObjects.Length; i++) {

            GaulItem newItemInfo = itemObjects[i] as GaulItem;
            if (newItemInfo == null) { Debug.LogError("Don't put other objects in the Data/Store folder pls."); }
            if (newItemInfo.canBuy == false) { continue; }
            
            itemInfo.Add(newItemInfo);

            StoreItemUI newItem = Instantiate(itemPrefab.gameObject, itemRect).GetComponent<StoreItemUI>();
            newItem.store = this;
            newItem.transform.SetSiblingIndex(itemTypeHeaders[(int)newItemInfo.itemType].transform.GetSiblingIndex()+1);

            items.Add(newItem);
            newItem.SetItem(newItemInfo);
        }

        XPComponent.OnLocalLevelUp += UpdateStore;
        CoinComponent.OnLocalUpdate += UpdateStore;
        Inventory.LocalInventory.OnUpdated += UpdateStore;
    }

    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);

        if(toggle) {
            notification.ToggleWindowClose();
            newItemCount = 0;
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        XPComponent.OnLocalLevelUp -= UpdateStore;
        CoinComponent.OnLocalUpdate -= UpdateStore;
        if(Inventory.LocalInventory) Inventory.LocalInventory.OnUpdated -= UpdateStore;
    }

    public void Buy(GaulItem buyItem, PaymentType paymentType) {
        if (buyItem == null) { Debug.LogError("No item"); return; }
        if (buyItem.ID < 0) { Debug.LogError("No item index"); return; }

        BuyItem(buyItem.ID, paymentType);
    }

    void UpdateStore() {

        int childCount = 0;
        int newChildCount = 0;

        for (int i = 0; i < items.Count; i++) {
            if(items[i].gameObject.activeSelf) { childCount++;}
            items[i].CanBuy();
            if(items[i].gameObject.activeSelf) { newChildCount++;}

        }

        if(newChildCount > childCount) {
            newItemCount += newChildCount - childCount;
            notification.UpdateField(newItemCount.ToString());
        }

        notification.ToggleWindow(!Active && newItemCount > 0);

    }

    public void BuyItem(int itemID, PaymentType payment) {
        // ActionsMUD.ActionOptimistic(PlayerComponent.LocalPlayer.Entity, ActionName.Buy, PlayerComponent.LocalPlayer.transform.position);
        TxManager.SendQueue<BuyFunction>(System.Convert.ToUInt32(itemID), System.Convert.ToByte((int)payment));
        BuyFX();
    }

    public void BuyFX() {
        SPUIBase.PlaySound(sfx_buy);
    }
}
