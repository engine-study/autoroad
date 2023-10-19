using System.Collections;
using System.Collections.Generic;
using mud;
using UnityEngine;

public class ProfileUI : EntityUI
{
    public static ProfileUI Instance;

    [Header("Profile")]
    public SPInputField nameField;
    public SPInputField publicKeyField;
    public SPButton addressField;

    public override void Init() {

        Instance = this;

        if(hasInit) {return;}

        base.Init();
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Instance = null;
    }

    protected override void UpdateEntity() {
        base.UpdateEntity();

        nameField.UpdateField(Entity.Name);
        publicKeyField.UpdateField(Entity.Key);

        PlayerComponent pc = Entity.GetMUDComponent<PlayerComponent>();
        addressField.ToggleWindow(pc != null);
        addressField.UpdateField(pc?.Address);

    }




}
