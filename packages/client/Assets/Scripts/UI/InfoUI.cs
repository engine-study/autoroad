using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using mud.Client;

public class InfoUI : SPWindow
{
    MUDEntity entity;

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
    public void UpdateInfo(Entity newEntity)
    {

        if(entity != newEntity) {
            if(entity != null)
                entity.OnUpdated -= Refresh;
            
            MUDEntity m = newEntity as MUDEntity;
            if(m != null) {
                m.OnUpdated += Refresh;
            }
        }
    
        entity = newEntity as MUDEntity;



        if(entity == null) {
            header.ToggleWindowClose();
            header.UpdateField("Empty");
        } else {
            header.ToggleWindowOpen();
            header.UpdateField(MUDHelper.TruncateHash(entity.Key));
        }

        // if(mEntity is Structure) {
        //     Structure structure = mEntity as Structure;

        // } else if(mEntity is Resource) {
        //     Resource resource = mEntity as Resource;

        // } else if (mEntity is Unit) {
        //     Unit unit = mEntity as Unit;

         // }

    }

    void Refresh() {
        UpdateInfo(entity);
    }
}
