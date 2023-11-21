using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class MoveTypeUI : MUDComponentUI
{
    [Header("Move")]
    public bool showZeroWeight;
    public bool showObstruction;
    public SPButton obstruction;
    public StatUI weight;
    public SPStrobeUI obstructing, tooHeavyStrobe;
    public Sprite obstructionSprite, permanentSprite;

    [Header("Debug")]
    public int weightValue;
    public MoveType moveValue;
    public MoveComponent moveComponent;
    public WeightComponent weightComponent;

    public override System.Type ComponentType() {return typeof(MoveComponent);}

    protected override void UpdateComponent() {
        base.UpdateComponent();

        moveComponent = Entity.GetMUDComponent<MoveComponent>();
        weightComponent = Entity.GetMUDComponent<WeightComponent>();
    
        if(moveComponent) {
            SetMove(moveComponent, weightComponent);
        } else {
            ToggleWindowClose();
        }

    }

    public void SetMove(MoveComponent m, WeightComponent w) { 

        moveValue = m.MoveType;
        weightValue = w ? w.Weight : 0;

        SetMove(moveValue, weightValue);

    }

    
    public void SetMove(MoveType moveType, float wValue, bool cannotMove = false) {
        
        StatType statValue = wValue >= 0 ? StatType.Weight : StatType.Strength;

        if(showObstruction) {
            obstruction.Image.sprite = moveType == MoveType.Obstruction ? obstructionSprite : permanentSprite;
            obstruction.ToggleWindow(moveType == MoveType.Obstruction || moveType == MoveType.Permanent);
        }

        if(moveType == MoveType.Push && (showZeroWeight || wValue != 0 )) {
            weight.SetValue(statValue, Mathf.Abs(wValue));
            weight.ToggleWindowOpen();
        } else {
            weight.ToggleWindowClose();
        }

        ToggleWindow(weight.gameObject.activeSelf || obstruction.gameObject.activeSelf);

        if(cannotMove && weight.gameObject.activeInHierarchy) {tooHeavyStrobe.StartStrobe();}
        else {tooHeavyStrobe.StopStrobe();}

        if(cannotMove && obstruction.gameObject.activeInHierarchy) {obstructing.StartStrobe();}
        else {obstructing.StopStrobe();}

    }



}
