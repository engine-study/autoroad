using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWorld.ContractDefinition;
using mud.Client;

public class StoreUI : SPWindowParent
{

    public AudioClip [] sfx_buy;

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
