using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

public class MoveTypeUI : MUDComponentUI
{
    [Header("Move")]
    public StatUI weight;
    public bool showObstruction;
    public SPButton obstruction;

    [Header("Debug")]
    public MoveComponent moveComponent;
    public WeightComponent weightComponent;

    public override System.Type ComponentType() {return typeof(MoveComponent);}

    protected override void UpdateComponent() {
        base.UpdateComponent();

        moveComponent = Entity.GetMUDComponent<MoveComponent>();
        weightComponent = Entity.GetMUDComponent<WeightComponent>();
    
        if(moveComponent) {


            if(showObstruction) {obstruction.ToggleWindow(moveComponent.MoveType == MoveType.Obstruction);}

            if(moveComponent.MoveType == MoveType.Push) {
                weight.SetValue(Mathf.Abs(weightComponent.Weight).ToString("00"));
                weight.ToggleWindowOpen();
            } else {
                weight.ToggleWindowClose();
            }

            ToggleWindow(weight.gameObject.activeSelf);
            

        } else {
            ToggleWindowClose();
        }

    }

}
