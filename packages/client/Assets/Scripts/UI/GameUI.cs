using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Unity;
using mud.Client;
using IWorld.ContractDefinition;
public class GameUI : SPWindowParent
{

    public SPButton coins;
    public SPButton scrolls;
    public SPButton storeButton;
    public SPButton debugButton;
    public SPButton menuButton;


    public override void Init() {
        base.Init();

        UpdateCoins();
        UpdateScrolls();

        CoinComponent.OnLocalUpdate += UpdateCoins;
        ScrollComponent.OnLocalUpdate += UpdateScrolls;
    }

    protected override void Destroy() {
        base.Destroy();

        CoinComponent.OnLocalUpdate -= UpdateCoins;
        ScrollComponent.OnLocalUpdate -= UpdateScrolls;

    }

    void UpdateCoins() {
        coins.UpdateField(CoinComponent.LocalCoins.ToString("00"));
    }

    void UpdateScrolls() {
        scrolls.UpdateField(ScrollComponent.LocalScrolls.ToString("00"));
    }


    public void Teleport() {

    }

    public void SendCoin() {

    }

    public void ToggleStore() {
        MotherUI.Mother.store.ToggleWindow();
    }

    public void ToggleMenu() {
        MotherUI.Mother.menu.ToggleWindow();
    }


    public void ToggleDebug() {
        MotherUI.Mother.debug.ToggleWindow();
    }


}
