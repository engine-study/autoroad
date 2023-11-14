using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;
using Nethereum.Web3.Accounts;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;


public class NetworkUI : SPWindow
{

    public static NetworkUI Instance;
    public NetworkData Network {get{return networkData;}}

    [Header("Account UI")]
    public NetworkField prefab;
    public SPButton confirm;
    public List<NetworkField> accounts;

    [Header("Debug")]
    public NetworkField selected;
    public NetworkData networkData;

    public override void Init() {

        if(hasInit) return;
        base.Init();

        Instance = this;

        prefab.ToggleWindowClose();
        
        if(NetworkManager.Initialized) {SetAddress();}
        else NetworkManager.OnInitialized += SetAddress;
    }

    protected override void Destroy() {
        base.Destroy();
        Instance = null;
        NetworkManager.OnInitialized -= SetAddress;
    }

    void SetAddress() {
        CreateAccount(NetworkManager.Account);
    }

    public void CreateAccount() {
        Account account = new Account(Common.GeneratePrivateKey(), NetworkManager.Network.chainId);
        CreateAccount(account);
    }

    public void CreateAccount(Account accountInfo) {

        NetworkField newAccount = Instantiate(prefab, transform);
        newAccount.transform.SetSiblingIndex(confirm.transform.GetSiblingIndex()-1);
        newAccount.SetAccount(accountInfo);
        newAccount.ToggleWindowOpen();
        SelectAccount(newAccount);
    }

    public void SelectAccount(NetworkField newAccount) {

        if(selected != null) {
            selected.networkName.ButtonText.fontStyle = TMPro.FontStyles.Normal;
        }

        selected = newAccount;
        selected.networkName.ButtonText.fontStyle = TMPro.FontStyles.Underline;

    }   

    public void ConfirmAccount() {

        if(selected == null) {return;}
        confirm.ToggleWindowClose();

        Sign();

    }

    async UniTask Sign() {
        DidSign(await TxManager.SendDirect<SupFunction>());
    }
    
    public void DidSign(bool sup) {
        
        if(sup) {
        
            PlayerPrefs.SetString("signed", NetworkManager.LocalAddress);
            PlayerPrefs.Save();
            
            //done, we have a legit account
            networkData = selected.networkData;
            // NetworkManager.Instance.SetAccount(account);
            ToggleWindowClose();

        } else {
            //try again, TX sign didn't go through
            confirm.ToggleWindowOpen();
        }

    }


}
