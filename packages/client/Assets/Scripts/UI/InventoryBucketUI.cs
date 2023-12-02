using System.Collections;
using System.Collections.Generic;
using mudworld;
using UnityEngine;

public class InventoryBucketUI : SPWindowEnumSelector
{
    public CosmeticType cosmetic;
    public CosmeticComponent component;
    protected override void Start() {
        base.Start();
        component = CosmeticComponent.Cosmetics[(int)cosmetic];
        
        OnEnable();

    }

    protected override void OnEnable() {
        base.OnEnable();

        if(component == null) {return;}

        for (int i = 0; i < buttons.Count; i++) {
            if(i >= component.Items.Count) {
                buttons[i].ToggleWindowClose();
            } else {
                buttons[i].UpdateField(component.Items[i].item.Name);
                bool owns = Inventory.LocalInventory.ItemUnlocked(component.Items[i].item);
                buttons[i].ToggleState( owns ? SPSelectableState.Default : SPSelectableState.Disabled);
            }
        }
    }

    protected override void UpdatedEnum(int index) {
        base.UpdatedEnum(index);
        CosmeticComponent.SetCosmetic(cosmetic, index);

    }
}
