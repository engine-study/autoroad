using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Unity.Rpc;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using mud;
using Nethereum.Web3.Accounts;

public class NetworkField : SPWindow
{
    [Header("Account")]
    [SerializeField] public SPButton networkName;
    [SerializeField] public SPButton networkContract;
    
    [Header("Debug")]
    [SerializeField] public NetworkData networkData;
    [SerializeField] public string nameField;
    [SerializeField] public string contractField;

    protected override void OnEnable() {
        base.OnEnable();

    }


    public void SetAccount(Account newAccount) {
        SetAddress(newAccount.Address);
    }

    public void SetAddress(string newAddress) {

    }

}
