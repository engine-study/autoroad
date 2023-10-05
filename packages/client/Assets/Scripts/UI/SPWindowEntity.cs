using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class SPWindowEntity : SPWindowBase
{
    [Header("Entity")]
    public mud.Client.MUDEntity entity;

    public virtual void SetEntity(mud.Client.MUDEntity newEntity) {
        
        //already setup this entity
        if(entity == newEntity) {
            return;
        }

        if(entity != null) {
            entity.OnUpdated -= Refresh;
        }
        
        entity = newEntity;

        //cant find component
        if(entity == null) {
            ToggleWindowClose();
            return;
        }

        entity.OnUpdated += Refresh;
        UpdateEntity();
    }

    public virtual void UpdateEntity() {
        ToggleWindowOpen();
    }

    
    void Refresh() {
        UpdateEntity();
    }

}
