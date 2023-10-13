using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;

public class ProfileUI : SPWindowParent
{
    [Header("Profile")]
    public PlayerUI playerUI;
    public SPInputField nameField;
    public SPInputField publicKeyField;

    protected override void Awake() {
        base.Awake();

        NameComponent.OnLocalName += UpdateName;
        NetworkManager.OnInitialized += UpdateAddress;
    }


    public override void ToggleWindow(bool toggle)
    {
        base.ToggleWindow(toggle);
        if(toggle) {
            playerUI.ShowLevel();
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        NameComponent.OnLocalName -= UpdateName;
        NetworkManager.OnInitialized -= UpdateAddress;

    }

    protected override void Start() {
        base.Start();

        UpdateAddress();
        UpdateName();
    }

    void UpdateAddress() {
        publicKeyField.UpdateField(NetworkManager.LocalAddressNotKey);
    }

    void UpdateName() {
        nameField.UpdateField(NameComponent.LocalName);
    }

}
