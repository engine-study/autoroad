using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class BarbarianComponent : MoverNPCComponent {


    [Header("Barbarian")]
    [SerializeField] SPAnimationProp propSword;
    [SerializeField] SPAnimationProp propBow;
    [SerializeField] RuntimeAnimatorController swordStance;
    [SerializeField] RuntimeAnimatorController bowStance;

    [Header("Debug")]
    [SerializeField] SPAnimator animator;

    protected override void PostInit() {
        base.PostInit();

        animator = GetComponentInChildren<SPAnimator>(true);

        if(npc.NPC == NPCType.Barbarian) {
            Entity.SetName("Barbarian");
            animator.SetDefaultProp(propSword);
            animator.SetDefaultController(swordStance);
        } else if(npc.NPC == NPCType.BarbarianArcher) {
            Entity.SetName("Archer");
            animator.SetDefaultProp(propBow);
            animator.SetDefaultController(bowStance);
        } else {
            Debug.LogError("Not a barbarian", this);
        }


    }

    protected override MUDTable GetTable() {return new BarbarianTable();}

}
