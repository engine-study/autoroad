using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MoveTypeUI : SPWindowMUDComponent
{
    [Header("Move")]
    public SPButton weight;
    public SPButton strength, obstacle;

    [Header("Debug")]
    public MoveComponent moveComponent;
    public WeightComponent weightComponent;

    public override System.Type ComponentType() {return typeof(MoveComponent);}

    public override void UpdateComponent() {
        base.UpdateComponent();

        moveComponent = entity.GetMUDComponent<MoveComponent>();
        weightComponent = entity.GetMUDComponent<WeightComponent>();
    
        if(moveComponent) {

            SPButton button = null;

            weight.ToggleWindowClose();
            strength.ToggleWindowClose();
            obstacle.ToggleWindowClose();

            if(moveComponent.MoveType == MoveType.Push) {
                button = weightComponent.Weight < 0 ? strength : weight;
                button.UpdateField(((int)Mathf.Abs(weightComponent.Weight)).ToString("00"));
                button.ToggleWindowOpen();
            } else if(moveComponent.MoveType == MoveType.Obstruction) {
                // button = obstacle;
                // button.ToggleWindowOpen();
            } else {

            }

            ToggleWindow(button != null);
            

        } else {
            ToggleWindowClose();
        }

    }

}
