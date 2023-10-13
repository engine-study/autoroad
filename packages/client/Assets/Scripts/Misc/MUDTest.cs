using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class MUDTest : mud.MUDNetworkSync
{

    public string worldAddress;
    public string blockNumber;
    protected override async void Init(mud.NetworkManager nm)
    {
        base.Init(nm);

        // worldAddress = nm.contractAddress;
        // blockNumber = nm.blockNumber;

        var jsonFile = Resources.Load<TextAsset>("latest");
        var data = JsonUtility.FromJson<mud.LocalDeploy>(jsonFile.text);
        worldAddress = data.worldAddress;

        Debug.Log("World: " + MUDHelper.Keccak256(worldAddress));
        Debug.Log("WorldEncoded: " + MUDHelper.Keccak256Address(worldAddress));
        Debug.Log("Position: " + MUDHelper.Keccak256(5, 10));
        Debug.Log("Combined: " + MUDHelper.Keccak256Address(worldAddress, 5, 10));
        Debug.Log("StringInt: " + MUDHelper.Keccak256("hello", 5, 10));
    }
}
