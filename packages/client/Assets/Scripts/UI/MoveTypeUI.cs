using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MoveTypeUI : SPWindowMUDComponent
{
    [Header("Move")]
    public StatUI weight;

    [Header("Debug")]
    public MoveComponent moveComponent;
    public WeightComponent weightComponent;

    public override System.Type ComponentType() {return typeof(MoveComponent);}

    public override void UpdateComponent() {
        base.UpdateComponent();

        moveComponent = Entity.GetMUDComponent<MoveComponent>();
        weightComponent = Entity.GetMUDComponent<WeightComponent>();
    
        if(moveComponent) {

            weight.ToggleWindowClose();

            if(moveComponent.MoveType == MoveType.Push) {
                weight.SetValue(Mathf.Abs(weightComponent.Weight).ToString("00"));
                weight.ToggleWindowOpen();
            } else if(moveComponent.MoveType == MoveType.Obstruction) {
                // button = obstacle;
                // button.ToggleWindowOpen();
            } else {

            }

            ToggleWindow(weight.gameObject.activeSelf);
            

        } else {
            ToggleWindowClose();
        }

    }

}
