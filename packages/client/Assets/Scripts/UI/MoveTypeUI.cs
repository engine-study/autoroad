using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MoveTypeUI : SPWindow
{
    [Header("Move")]
    public SPWindowPosition position;
    public SPButton weight, strength, obstacle;
    public MUDEntity entity;
    public MoveComponent moveComponent;
    public WeightComponent weightComponent;
    public void UpdateInfo(Entity newEntity) {

        if(entity != newEntity) {
            if(entity != null)
                entity.OnUpdated -= Refresh;
            
            MUDEntity m = (MUDEntity)newEntity;
            if(m != null) {
                m.OnUpdated += Refresh;
            }
        }

        entity = (MUDEntity)newEntity;

        if (entity) {
            moveComponent = entity.GetMUDComponent<MoveComponent>();
            weightComponent = entity.GetMUDComponent<WeightComponent>();
        }

        if(entity && moveComponent) {

            SPButton button = null;

            weight.ToggleWindowClose();
            strength.ToggleWindowClose();
            obstacle.ToggleWindowClose();

            if(moveComponent.MoveType == MoveType.Obstruction) {
                button = obstacle;
                obstacle.ToggleWindowClose();
            } else if(moveComponent.MoveType == MoveType.Push) {
                button = weightComponent.Weight < 0 ? strength : weight;
                button.UpdateField(((int)Mathf.Abs(weightComponent.Weight)).ToString());
            } 

            if(button == null) {
                ToggleWindowClose();
            } else {
                button.ToggleWindowOpen();
            }

            ToggleWindowOpen();

        } else {
            ToggleWindowClose();
        }

    }

    void Refresh() {
        UpdateInfo(entity);
    }
}
