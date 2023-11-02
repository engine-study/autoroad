using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud;
using mud;
using IWorld.ContractDefinition;
public class GameUI : SPWindowParent
{

    public StatUI coins;
    public StatUI gems;
    public StatUI scrolls;
    public StatUI seeds;
    public SPButton storeButton;
    public SPButton debugButton;
    public SPButton menuButton;
    
    public TeleportUI teleportUI;


    public override void Init() {
        base.Init();

        UpdateCoins();
        UpdateGems();
        UpdateScrolls();
        UpdateSeeds();

        teleportUI.ToggleWindowClose();

        GemComponent.OnLocalUpdate += UpdateGems;
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

    void UpdateGems() {
        gems.SetValue(GemComponent.LocalGems);
        SPStrobeUI.ToggleStrobe(gems);
    }

    void UpdateCoins() {
        coins.SetValue(CoinComponent.LocalCoins);
        SPStrobeUI.ToggleStrobe(coins);
    }

    void UpdateScrolls() {
        scrolls.SetValue(ScrollComponent.LocalScrolls);
        scrolls.Button.ToggleState(ScrollComponent.LocalScrolls > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(scrolls);
    }

     void UpdateSeeds() {
        seeds.SetValue(SeedComponent.LocalCount);
        seeds.Button.ToggleState(SeedComponent.LocalCount > 0 ? SPSelectableState.Default : SPSelectableState.Disabled);
        SPStrobeUI.ToggleStrobe(seeds);
    }


    public void SendCoin() {

    }

}
