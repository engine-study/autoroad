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
    public SPButton seeds;
    public SPButton storeButton;
    public SPButton debugButton;
    public SPButton menuButton;

    public GameObject teleportUI;


    public override void Init() {
        base.Init();

        UpdateCoins();
        UpdateScrolls();
        UpdateSeeds();

        teleportUI.SetActive(false);

        CoinComponent.OnLocalUpdate += UpdateCoins;
        ScrollComponent.OnLocalUpdate += UpdateScrolls;
        SeedsComponent.OnLocalUpdate += UpdateSeeds;
    }

    protected override void Destroy() {
        base.Destroy();

        CoinComponent.OnLocalUpdate -= UpdateCoins;
        ScrollComponent.OnLocalUpdate -= UpdateScrolls;
        SeedsComponent.OnLocalUpdate -= UpdateSeeds;

    }

    void UpdateCoins() {
        coins.UpdateField(CoinComponent.LocalCoins.ToString("00"));
        SPStrobeUI.ToggleStrobe(coins);
    }

    void UpdateScrolls() {
        scrolls.UpdateField(ScrollComponent.LocalScrolls.ToString("00"));
        scrolls.ToggleState(ScrollComponent.LocalScrolls > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(scrolls);
    }

     void UpdateSeeds() {
        seeds.UpdateField(SeedsComponent.LocalCount.ToString("00"));
        seeds.ToggleState(SeedsComponent.LocalCount > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(seeds);
    }


    public void SendCoin() {

    }

    public void ToggleStore() {
        MotherUI.Mother.ToggleMenuWindow(MotherUI.Mother.store);
    }

    public void ToggleMenu() {
        MotherUI.Mother.ToggleMenuWindow(MotherUI.Mother.menu);
    }


    public void ToggleDebug() {
        MotherUI.Mother.ToggleMenuWindow(MotherUI.Mother.debug);
    }

    public void ToggleTeleport() {
        teleportUI.SetActive(!teleportUI.activeSelf);
    }
    public void ToggleTeleport(bool toggle) {
        teleportUI.SetActive(toggle);
    }

}
