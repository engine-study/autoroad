using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class CarriageComponent : MUDComponent
{
    [Header("Carriage")]
    public SPAudioSource moveSound;
    public Animator horseWalk;
    public PositionSync sync;
    

    protected override void PostInit() {
        base.PostInit();

        sync.OnMoveStart += WagonFX;
        sync.OnMoveEnd += WagonEndFX;

    }

    protected override void OnDestroy() {
        base.OnDestroy();
        
        sync.OnMoveStart -= WagonFX;
        sync.OnMoveEnd -= WagonEndFX;
    }
    protected override MUDTable GetTable() {return new CarriageTable();}
    protected override void UpdateComponent(MUDTable table, UpdateInfo newInfo) {
        // throw new System.NotImplementedException();

        Entity.SetName("Carriage");
    }

    void WagonFX() {
        horseWalk.enabled = true;
        moveSound.Play();
    }

    void WagonEndFX() {
        horseWalk.enabled = false;
    }

}
