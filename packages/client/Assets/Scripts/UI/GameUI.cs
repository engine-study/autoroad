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
    public SPButton storeButton;
    public SPButton debugButton;
    public SPButton menuButton;
    
    public TeleportUI teleportUI;


    public override void Init() {
        if(HasInit) {return;}
        base.Init();

        teleportUI.ToggleWindowClose();

        GemComponent.OnLocalUpdate += UpdateGems;
        CoinComponent.OnLocalUpdate += UpdateCoins;

    }

    protected override void Destroy() {
        base.Destroy();

        GemComponent.OnLocalUpdate -= UpdateGems;
        CoinComponent.OnLocalUpdate -= UpdateCoins;

    }

    void UpdateGems() {
        gems.SetValue(GemComponent.LocalGems);
        SPStrobeUI.ToggleStrobe(gems);
    }

    void UpdateCoins() {
        coins.SetValue(CoinComponent.LocalCoins);
        SPStrobeUI.ToggleStrobe(coins);
    }

    public void SendCoin() {

    }

}
