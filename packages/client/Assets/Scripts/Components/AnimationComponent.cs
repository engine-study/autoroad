using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public enum AnimationType {Walk, Hop, Teleport}

public class AnimationComponent : MUDComponent {

    public AnimationType State {get { return animType; } }

    [Header("State")]
    [SerializeField] private AnimationType animType;
    [SerializeField] private PositionComponent pos;


    protected override void PostInit() {
        base.PostInit();

        pos = Entity.GetMUDComponent<PositionComponent>();

    }
    

    protected override IMudTable GetTable() {return new AnimationTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        AnimationTable table = update as AnimationTable;
        animType = (AnimationType)table.state;

    }



}
