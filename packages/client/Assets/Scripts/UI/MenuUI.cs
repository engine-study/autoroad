using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;

public class MenuUI : SPWindowParent
{
    public SPButton testnet, mainnet;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(NetworkManager.Instance == null) return;

        testnet.ToggleWindow(NetworkManager.Instance.networkType != NetworkTypes.NetworkType.Testnet);
        mainnet.ToggleWindow(NetworkManager.Instance.networkType != NetworkTypes.NetworkType.Mainnet);
    }
    
    public void ToggleQuality() {
        GameState.Instance.ToggleQuality();
    }

    public void GoToTestnet() {
        NetworkManager.Instance.SwitchNetwork(NetworkManager.Instance.testnet);
    }

    public void GoToMainnet() {
        NetworkManager.Instance.SwitchNetwork(NetworkManager.Instance.mainnet);
    }
}
