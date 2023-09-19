using System.Collections;
using System.Collections.Generic;
using mud.Unity;
using UnityEngine;

public class ProfileUI : SPWindowParent
{
    [Header("Profile")]
    public SPInputField nameField;
    public SPInputField publicKeyField;

    protected override void Awake() {
        base.Awake();

        NameComponent.OnLocalName += UpdateName;
        NetworkManager.OnInitialized += UpdateAddress;
    }

    protected override void OnDisable() {
        base.OnDisable();

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
