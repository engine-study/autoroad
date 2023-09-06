using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWorld.ContractDefinition;
using mud.Client;

public class StoreUI : SPWindowParent
{

    [Header("Store")]
    public StoreItemUI itemPrefab;
    public AudioClip [] sfx_buy;

    [Header("Debug")]
    public List<GaulItem> itemInfo = new List<GaulItem>();
    public List<StoreItemUI> items = new List<StoreItemUI>();
    

    protected override void Start() {
        base.Start();

        object[] itemObjects = Resources.LoadAll("Data/Store");
        itemPrefab.ToggleWindowClose();

        for (int i = 0; i < itemObjects.Length; i++) {

            GaulItem newItemInfo = itemObjects[i] as GaulItem;
            if (newItemInfo == null) { Debug.LogError("Don't put other objects in the Data/Store folder pls."); }
            
            itemInfo.Add(newItemInfo);

            StoreItemUI newItem = Instantiate(itemPrefab.gameObject, transform).GetComponent<StoreItemUI>();
            items.Add(newItem);

            newItem.SetItem(newItemInfo);
        }

        XPComponent.OnLevelUp += UpdateStore;
        CoinComponent.OnLocalUpdate += UpdateStore;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        XPComponent.OnLevelUp -= UpdateStore;
        CoinComponent.OnLocalUpdate -= UpdateStore;
    }


    void UpdateStore() {
        for (int i = 0; i < items.Count; i++) {
            items[i].CanBuy();
        }
    }

    public void BuyScroll() {
        TxManager.Send<BuyScrollFunction>();
        BuyFX();
    }    

    public void BuyItem(int itemID) {
        TxManager.Send<BuyCosmeticFunction>(System.Convert.ToUInt32(itemID));
        BuyFX();
    }

    public void BuyFX() {
        SPUIBase.PlaySound(sfx_buy);
    }
}
