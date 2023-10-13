using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Unity;
using mud;
public class WorldUI : SPWindowParent
{
    [SerializeField] SPRawText burnerAddress, worldContract, faucet;

    protected override void Awake() {
        base.Awake();

        NetworkManager.OnInitialized += Display;
    }

    void Display() {
        burnerAddress.UpdateField(NetworkManager.LocalAddressNotKey);
        worldContract.UpdateField(MUDHelper.TruncateHash(NetworkManager.WorldAddress) + "(" + NetworkManager.Network.chainId + ") " + NetworkManager.Network.jsonRpcUrl);
        // chainID.UpdateField(NetworkManager.Network.chainId);
    }
}
