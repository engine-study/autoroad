using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowEntity : SPWindow
{
    [Header("Entity")]
    public Entity entity;
    public SPWindowPosition position;


    public override void Init()
    {
        base.Init();
    }
    public virtual void UpdateEntity(Entity newEntity) {
        entity = newEntity;
        if(entity) {
            if(position) {
                position.SetFollow(entity.transform);   
            }

        } else {

        }
    }
}
