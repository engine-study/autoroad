using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
public class WorldUI : SPWindowParent
{
    [SerializeField] SPRawText burnerAddress, worldContract, faucet;

    protected override void Awake() {
        base.Awake();

        NetworkManager.OnInitialized += Display;
    }

    void Display() {
        burnerAddress.UpdateField(NetworkManager.LocalAddress);
        worldContract.UpdateField(MUDHelper.TruncateHash(NetworkManager.WorldAddress) + "(" + NetworkManager.ActiveNetwork.chainId + ") " + NetworkManager.ActiveNetwork.jsonRpcUrl);
        // chainID.UpdateField(NetworkManager.Network.chainId);
    }
}
