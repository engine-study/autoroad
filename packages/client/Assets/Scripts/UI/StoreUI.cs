using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IWorld.ContractDefinition;
using mud.Client;

public class StoreUI : SPWindowParent
{

    public void BuyScroll() {
        TxManager.Send<BuyScrollFunction>();
    }    

    public void BuyItem(int itemID) {
        TxManager.Send<BuyCosmeticFunction>(itemID);
    }
}
