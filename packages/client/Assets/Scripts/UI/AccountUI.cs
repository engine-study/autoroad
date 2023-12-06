using System.Collections;
using System.Collections.Generic;
using mud;
using mudworld;
using UnityEngine;
using Nethereum.Web3.Accounts;
using Cysharp.Threading.Tasks;
using IWorld.ContractDefinition;


public class AccountUI : SPWindow
{

    public const string SIGNATURE = "signed";
    public static AccountUI Instance;
    public Account Account {get{return account;}}

    [Header("Account UI")]
    public AccountField prefab;
    public SPButton newAccountButton;
    public SPInputField accountCopyPaste;
    public SPButton confirm;
    public List<AccountField> accounts;

    [Header("Debug")]
    public AccountField selected;
    public Account account;

    public static bool HAS_SIGNED {get {return PlayerPrefs.GetString(SIGNATURE) == NetworkManager.LocalAddress;}}

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
        LoadAccount(NetworkManager.Account);
    }

    public void CreateAccount() {
        LoadAccount(NetworkManager.CreateAccount(Common.GeneratePrivateKey()));
    }

    public void LoadAccount(Account accountInfo) {

        AccountField newAccount = Instantiate(prefab, transform);
        newAccount.transform.SetSiblingIndex(newAccountButton.transform.GetSiblingIndex()-1);
        newAccount.SetAccount(accountInfo);
        newAccount.ToggleWindowOpen();
        SelectAccount(newAccount);
    }

    public void SelectAccount(AccountField newAccount) {

        if(selected != null) {
            selected.address.ButtonText.fontStyle = TMPro.FontStyles.Normal;
        }

        selected = newAccount;
        selected.address.ButtonText.fontStyle = TMPro.FontStyles.Underline;

        accountCopyPaste.UpdateField(newAccount.account.Address);
    }   

    public async void ConfirmAccount() {

        if(selected == null) {return;}
        ToggleWindowClose();

        await SetNetworkAndDrip();

    }

    async UniTask SetNetworkAndDrip() {

        await NetworkManager.SetAccount(selected.account);
        await UniTask.Delay(100);
        await Sign();

    }

    async UniTask Sign() {
        // DidSign(await TxManager.SendDirect<SupFunction>());
        DidSign(await TxManager.SendDirect<BuyFunction>(System.Convert.ToUInt32(6), System.Convert.ToByte(PaymentType.Eth)));

    }
    
    public void DidSign(bool sup) {
        
        if(sup) {
        
            PlayerPrefs.SetString(SIGNATURE, NetworkManager.LocalAddress);
            PlayerPrefs.Save();
            
            //done, we have a legit account
            account = selected.account;
            // NetworkManager.Instance.SetAccount(account);
            ToggleWindowClose();

            Debug.Log($"Account signed: {NetworkManager.LocalAddress}");

        } else {
            //try again, TX sign didn't go through
            ToggleWindowOpen();
            Debug.LogError($"Account didn't sign: {NetworkManager.LocalAddress}");
        }

    }

}
