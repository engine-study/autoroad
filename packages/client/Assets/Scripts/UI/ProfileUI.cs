using System.Collections;
using System.Collections.Generic;
using mud.Unity;
using UnityEngine;

public class ProfileUI : SPWindow
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

    void UpdateAddress() {
        publicKeyField.UpdateField(NetworkManager.LocalAddress);
    }

    void UpdateName() {
        nameField.UpdateField(NameComponent.LocalName);
    }

}
