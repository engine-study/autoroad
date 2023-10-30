using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class EntityUI : SPWindow
{
    public MUDEntity Entity {get{return entity;}}
    public SPWindowPosition Position {get{return position;}}

    [Header("Entity")]
    [SerializeField] MUDEntity entity;
    [SerializeField] protected SPWindowPosition position;

    public virtual void SetEntity(MUDEntity newEntity, bool force = false) {
        
        if(!hasInit) Init();
        
        //already setup this entity
        if(Entity == newEntity && !force) { return;}
        if(Entity != null) { Entity.OnUpdated -= Refresh;}
        
        entity = newEntity;

        //cant find component
        if(Entity == null) {
            ToggleWindowClose();
            return;
        }

        Entity.OnUpdated += Refresh;
        UpdateEntity();
    }

    protected virtual void UpdateEntity() {
        ToggleWindowOpen();
    }

    
    void Refresh() {
        UpdateEntity();
    }

}
