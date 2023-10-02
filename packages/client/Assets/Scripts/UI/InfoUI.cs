using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud;

public class InfoUI : SPWindowEntity
{

    [Header("Info")]
    public SPButton header;
    public SPButton playerName, coordinate;
    public SPRawText text;

    protected override void Awake() {
        base.Awake();
        header.ToggleWindowClose();
        coordinate.ToggleWindowClose();
        playerName.ToggleWindowClose();
    }
    public void UpdateCoordinate(int x, int y) {
        coordinate.ToggleWindowOpen();
        coordinate.UpdateField("(" + x + "," + y + ")");
    }

    public void ClearCoordinate() {
        coordinate.UpdateField("? ?");
    }

    public override void SetEntity(MUDEntity newEntity)
    {
        base.SetEntity(newEntity);

        if(entity == null) {
            header.ToggleWindowClose();
            header.UpdateField("Empty");
        } else {
            header.ToggleWindowOpen();
            header.UpdateField(MUDHelper.TruncateHash(entity.Key));
        }

    }

}
