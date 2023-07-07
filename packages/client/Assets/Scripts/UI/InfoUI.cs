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

    public void UpdateInfo(Entity newEntity, int x, int y)
    {

        MUDEntity mEntity = newEntity as MUDEntity;

        if(mEntity == null) {
            ToggleWindowClose();
            return;
        }

        header.UpdateField(mEntity.gameObject.name);
        coordinate.UpdateField("[ " + x + " ] [ " + y + " ]");

        if(mEntity is Structure) {
            Structure structure = mEntity as Structure;

        } else if(mEntity is Resource) {
            Resource resource = mEntity as Resource;

        } else if (mEntity is Unit) {
            Unit unit = mEntity as Unit;

        }


    }
}
