using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MUDTest : mud.Client.MUDNetworkSync
{

    protected override async void Init(mud.Unity.NetworkManager nm)
    {
        base.Init(nm);

        Debug.Log("World" + MUDHelper.GetSha3ABIEncodedAddress(nm.contractAddress));
        Debug.Log("Position" + MUDHelper.GetSha3ABIEncodedPacked(5, 10));
    }
}
