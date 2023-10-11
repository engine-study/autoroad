using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class SPWindowEntity : SPWindow
{
    public MUDEntity Entity {get{return entity;}}

    [Header("Entity")]
    [SerializeField] MUDEntity entity;
    [SerializeField] protected SPWindowPosition position;

    public virtual void SetEntity(MUDEntity newEntity) {
        
        //already setup this entity
        if(Entity == newEntity) {
            return;
        }

        if(Entity != null) {
            Entity.OnUpdated -= Refresh;
        }
        
        entity = newEntity;

        //cant find component
        if(Entity == null) {
            ToggleWindowClose();
            return;
        }

        Entity.OnUpdated += Refresh;
        UpdateEntity();
    }

    public virtual void UpdateEntity() {
        ToggleWindowOpen();
    }

    
    void Refresh() {
        UpdateEntity();
    }

}
