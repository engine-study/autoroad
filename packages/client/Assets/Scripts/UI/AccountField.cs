using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Unity.Rpc;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using mud;
using mudworld;
using Nethereum.Web3.Accounts;

public class AccountField : SPWindow
{
    [Header("Account")]
    [SerializeField] public SPButton address;
    [SerializeField] public StatUI eth;
    [SerializeField] float ethBalance;
    
    [Header("Debug")]
    [SerializeField] public Account account;
    [SerializeField] public string addressField;

    Coroutine update;

    // public void SetAccount()


    protected override void OnEnable() {
        base.OnEnable();

        eth.SetValue(StatType.Eth, "?");
        update = StartCoroutine(GetBalance());

    }


    public void SetAccount(Account newAccount) {
        account = newAccount;
        SetAddress(newAccount.Address);
    }

    public void SetAddress(string newAddress) {
        addressField = newAddress;
        address.UpdateField(newAddress);
        
        NameTable nameTable = IMudTable.GetTable<NameTable>(NetworkManager.AccountKey(newAddress));
        if(nameTable == null) {
            address.UpdateField("New Player");
        } else {
            address.UpdateField(NameUI.TableToName((int)nameTable.First, (int)nameTable.Middle, (int)nameTable.Last));
        }

    }

    private IEnumerator GetBalance() {

        while(NetworkManager.Initialized == false) {yield return null;}

        while(true) {

            var balanceRequest = new EthGetBalanceUnityRequest(NetworkManager.Network.jsonRpcUrl);
            yield return balanceRequest.SendRequest(addressField, BlockParameter.CreateLatest());
            ethBalance = (float)UnitConversion.Convert.FromWei(balanceRequest.Result.Value);

            eth.SetValue(StatType.Eth, ethBalance);
            
            yield return new WaitForSeconds(1f);
        }

    }
}
