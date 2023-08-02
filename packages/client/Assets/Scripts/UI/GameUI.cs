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

    public GameObject teleportUI;


    public override void Init() {
        base.Init();

        UpdateCoins();
        UpdateScrolls();

        teleportUI.SetActive(false);

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
        scrolls.ToggleState(ScrollComponent.LocalScrolls > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
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
