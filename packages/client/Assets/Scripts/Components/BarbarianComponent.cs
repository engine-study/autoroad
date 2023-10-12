using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class BarbarianComponent : MoverComponent {


    [Header("Barbarian")]
    [SerializeField] SPAnimationProp propSword;
    [SerializeField] SPAnimationProp propBow;
    [SerializeField] RuntimeAnimatorController swordStance;
    [SerializeField] RuntimeAnimatorController bowStance;

    [Header("Debug")]
    [SerializeField] NPCComponent npc;
    [SerializeField] SPAnimator animator;

    protected override void PostInit() {
        base.PostInit();

        npc = Entity.GetMUDComponent<NPCComponent>();
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

    protected override IMudTable GetTable() {return new BarbarianTable();}

}
