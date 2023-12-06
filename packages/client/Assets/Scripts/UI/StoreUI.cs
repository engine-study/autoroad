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
    public List<GaulItem>[] itemCategories;

    protected override void Start() {
        base.Start();

        itemPrefab.ToggleWindowClose();
        itemCategories = new List<GaulItem>[itemTypeHeaders.Length];

        for(int i = 0; i < itemTypeHeaders.Length; i++) {
            // itemTypeHeaders[i].UpdateField(GaulItem.ItemTypeString((ItemType)i));
            itemCategories[i] = new List<GaulItem>();
        }

        GaulItem[] itemObjects = Resources.LoadAll<GaulItem>("Data/Store");

        for (int i = 0; i < itemObjects.Length; i++) {

            GaulItem newItemInfo = itemObjects[i];
            if (newItemInfo == null) { Debug.LogError("Don't put other objects in the Data/Store folder pls."); }
            if (newItemInfo.canBuy == false) { continue; }

            itemInfo.Add(newItemInfo);
            itemCategories[(int)newItemInfo.itemType].Add(newItemInfo);
        }

        //sort all items by cost
        for(int i = 0; i < itemCategories.Length; i++) {
            itemCategories[i].Sort((a, b) => b.CompareTo(a));
        }

        //spawn store objects
        for(int i = 0; i < itemCategories.Length; i++) {
        
            SPButton header = itemTypeHeaders[i];

            for (int j = 0; j < itemCategories[i].Count; j++) {
                
                StoreItemUI newItem = Instantiate(itemPrefab.gameObject, header.Rect.parent).GetComponent<StoreItemUI>();
                newItem.transform.SetSiblingIndex(header.transform.GetSiblingIndex()+1);

                newItem.store = this;

                items.Add(newItem);
                newItem.SetItem(itemCategories[i][j]);
            }
        }

        UpdateStore();

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
