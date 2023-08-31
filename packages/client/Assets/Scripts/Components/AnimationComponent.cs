using UnityEngine;
using DefaultNamespace;
using mud.Client;
using mud.Unity;
using System;

public enum AnimationType {Walk, Hop, Teleport}

public class AnimationComponent : MUDComponent {

    public AnimationType Anim {get { return animType; } }

    [Header("State")]
    [SerializeField] AnimationType animType;
    [SerializeField] PositionComponent pos;

    [EnumNamedArray( typeof(AnimationType) )]
    [SerializeField] SPLerpCurve [] lerpType;

    [EnumNamedArray( typeof(AnimationType) )]
    [SerializeField] SPEnableDisable [] effects;


    protected override void Awake() {
        base.Awake();
        for (int i = 0; i < effects.Length; i++) {
            effects[i].gameObject.SetActive(false);
        }
    }
    protected override void PostInit() {
        base.PostInit();

        pos = Entity.GetMUDComponent<PositionComponent>();

    }
    

    protected override IMudTable GetTable() {return new AnimationTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        AnimationTable table = update as AnimationTable;
        animType = (AnimationType)table.state;

        // PlayAnimation();

    }

    public void PlayAnimation() {
        Debug.Log("Animation: " + animType.ToString());
        for (int i = 0; i < effects.Length; i++) {
            if(i == (int)animType) {
                if(effects[i].gameObject.activeInHierarchy) {
                    effects[i].PlayEnabled();
                } else {
                    effects[i].gameObject.SetActive(true);
                }
            } else {
                effects[i].gameObject.SetActive(false);
            }
        }
        
    }


}
