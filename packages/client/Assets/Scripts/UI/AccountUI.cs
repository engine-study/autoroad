using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;
using Nethereum.Unity.Rpc;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

public class AccountUI : SPWindow
{
    [Header("Account")]
    [SerializeField] SPButton address;
    [SerializeField] StatUI eth;
    [SerializeField] float ethBalance;

    Coroutine update;
    public override void Init() {

        if(hasInit) return;
        base.Init();

        if(NetworkManager.Initialized) {SetAddress();}
        else NetworkManager.OnInitialized += SetAddress;
    }

    protected override void Destroy() {
        base.Destroy();
        NetworkManager.OnInitialized -= SetAddress;
    }

    protected override void OnEnable() {
        base.OnEnable();

        eth.SetValue(StatType.Eth, "?");
        update = StartCoroutine(GetBalance());

    }

    void SetAddress() {
        address.UpdateField(NetworkManager.LocalAddress);
    }

    private IEnumerator GetBalance() {

        while(NetworkManager.Initialized == false) {yield return null;}

        while(true) {

            var balanceRequest = new EthGetBalanceUnityRequest(NetworkManager.Network.jsonRpcUrl);
            yield return balanceRequest.SendRequest(NetworkManager.LocalAddress, BlockParameter.CreateLatest());
            ethBalance = (float)UnitConversion.Convert.FromWei(balanceRequest.Result.Value);

            eth.SetValue(StatType.Eth, ethBalance);
            
            yield return new WaitForSeconds(1f);
        }

    }
}
