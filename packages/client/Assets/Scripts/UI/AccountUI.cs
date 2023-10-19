using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;
using Nethereum.Web3.Accounts;


public class AccountUI : SPWindow
{

    public static AccountUI Instance;
    public Account Account {get{return account;}}

    [Header("Account UI")]
    public AccountField prefab;
    public SPButton confirm;
    public List<AccountField> accounts;

    [Header("Debug")]
    public AccountField selected;
    public Account account;

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
        CreateAccount(NetworkManager.Instance.Account);
    }

    public void CreateAccount() {
        Account account = new Account(Common.GeneratePrivateKey(), NetworkManager.Network.chainId);
        CreateAccount(account);
    }

    public void CreateAccount(Account accountInfo) {

        AccountField newAccount = Instantiate(prefab, transform);
        newAccount.transform.SetSiblingIndex(confirm.transform.GetSiblingIndex()-1);
        newAccount.SetAccount(accountInfo);
        newAccount.ToggleWindowOpen();
        SelectAccount(newAccount);
    }

    public void ConfirmAccount() {

        if(selected == null) {return;}
        
        account = selected.account;
        NetworkManager.Instance.SetAccount(account);
        
        ToggleWindowClose();

    }

    public void SelectAccount(AccountField newAccount) {

        if(selected != null) {
            selected.address.ButtonText.fontStyle = TMPro.FontStyles.Normal;
        }

        selected = newAccount;
        selected.address.ButtonText.fontStyle = TMPro.FontStyles.Underline;


    }   

}
