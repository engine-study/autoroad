using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class InfoUI : SPWindow
{
    [Header("Info")]
    public SPHeading header;
    public SPHeading playerName, coordinate;
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
    public void UpdateInfo(Entity newEntity)
    {

        MUDEntity mEntity = newEntity as MUDEntity;

        if(mEntity == null) {
            header.ToggleWindowClose();
            header.UpdateField("Empty");
        } else {
            header.ToggleWindowOpen();
            header.UpdateField(MUDHelper.TruncateHash(mEntity.Key));
        }

        // if(mEntity is Structure) {
        //     Structure structure = mEntity as Structure;

        // } else if(mEntity is Resource) {
        //     Resource resource = mEntity as Resource;

        // } else if (mEntity is Unit) {
        //     Unit unit = mEntity as Unit;

         // }


    }
}
