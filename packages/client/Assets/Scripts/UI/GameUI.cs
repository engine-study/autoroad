using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : SPWindowParent
{

    public SPButton coins;
    public SPButton scrolls;
    public SPButton storeButton;
    public SPButton menuButton;


    public override void Init() {
        base.Init();

        UpdateCoins();
        CoinComponent.OnLocalUpdate += UpdateCoins;
        ScrollComponent.OnLocalUpdate += UpdateScrolls;
    }

    protected override void Destroy() {
        base.Destroy();

        CoinComponent.OnLocalUpdate -= UpdateCoins;
        ScrollComponent.OnLocalUpdate -= UpdateScrolls;

    }

    void UpdateCoins() {
        coins.UpdateField(CoinComponent.LocalCoins.ToString());
    }

    void UpdateScrolls() {
        scrolls.UpdateField(ScrollComponent.LocalScrolls.ToString());
    }

    public void ToggleStore() {
        MotherUI.Mother.store.ToggleWindow();
    }

    public void ToggleMenu() {
        MotherUI.Mother.menu.ToggleWindow();
    }

    
}
