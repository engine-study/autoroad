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
            newItem.store = this;
            items.Add(newItem);

            newItem.SetItem(newItemInfo);
        }

        XPComponent.OnLocalLevelUp += UpdateStore;
        CoinComponent.OnLocalUpdate += UpdateStore;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        XPComponent.OnLocalLevelUp -= UpdateStore;
        CoinComponent.OnLocalUpdate -= UpdateStore;
    }

    public void Buy(GaulItem buyItem, PaymentType paymentType) {
        if (buyItem == null) { Debug.LogError("No item"); return; }
        if (buyItem.ID < 0) { Debug.LogError("No item index"); return; }

        BuyItem(buyItem.ID, paymentType);
    }

    void UpdateStore() {
        for (int i = 0; i < items.Count; i++) {
            items[i].CanBuy();
        }
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
