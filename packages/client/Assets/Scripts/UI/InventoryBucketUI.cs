using System.Collections;
using System.Collections.Generic;
using mudworld;
using UnityEngine;

public class InventoryBucketUI : SPWindowEnumSelector
{
    public CosmeticType cosmetic;
    protected override void UpdatedEnum(int index) {
        base.UpdatedEnum(index);
        CosmeticComponent.UpdateCosmetic(cosmetic, index);
    }
}
