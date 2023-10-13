using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class MoveTypeUI : MUDComponentUI
{
    [Header("Move")]
    public bool showObstruction;
    public bool showZeroWeight;
    public SPButton obstruction;
    public StatUI weight;

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

            if(moveComponent.MoveType == MoveType.Push && weightComponent && (showZeroWeight || weightComponent.Weight != 0 )) {

                weight.SetValue(weightComponent.Weight >= 0 ? StatType.Weight : StatType.Strength, Mathf.Abs(weightComponent.Weight).ToString("00"));
                weight.ToggleWindowOpen();
            } else {
                weight.ToggleWindowClose();
            }

            ToggleWindow(weight.gameObject.activeSelf || obstruction.gameObject.activeSelf);
            

        } else {
            ToggleWindowClose();
        }

    }

}
