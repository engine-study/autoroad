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

    public StatUI coins;
    public StatUI scrolls;
    public StatUI seeds;
    public SPButton storeButton;
    public SPButton debugButton;
    public SPButton menuButton;
    
    public TeleportUI teleportUI;


    public override void Init() {
        base.Init();

        UpdateCoins();
        UpdateScrolls();
        UpdateSeeds();

        teleportUI.ToggleWindowClose();

        CoinComponent.OnLocalUpdate += UpdateCoins;
        ScrollComponent.OnLocalUpdate += UpdateScrolls;
        SeedComponent.OnLocalUpdate += UpdateSeeds;
    }

    protected override void Destroy() {
        base.Destroy();

        CoinComponent.OnLocalUpdate -= UpdateCoins;
        ScrollComponent.OnLocalUpdate -= UpdateScrolls;
        SeedComponent.OnLocalUpdate -= UpdateSeeds;

    }

    void UpdateCoins() {
        coins.SetValue(CoinComponent.LocalCoins.ToString("000"));
        SPStrobeUI.ToggleStrobe(coins);
    }

    void UpdateScrolls() {
        scrolls.SetValue(ScrollComponent.LocalScrolls.ToString("00"));
        scrolls.Button.ToggleState(ScrollComponent.LocalScrolls > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(scrolls);
    }

     void UpdateSeeds() {
        seeds.SetValue(SeedComponent.LocalCount.ToString("00"));
        seeds.Button.ToggleState(SeedComponent.LocalCount > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(seeds);
    }


    public void SendCoin() {

    }

}
