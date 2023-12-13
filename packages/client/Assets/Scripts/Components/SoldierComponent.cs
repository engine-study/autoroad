using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using UnityEditor.Animations;

public class SoldierComponent : MoverNPCComponent
{
    public static NPCType [] SoldierTypes = {NPCType.Soldier, NPCType.Proctor};

    [Header("Soldier")]
    public GameObject [] heads;
    public GameObject [] bodies;
    public SPAnimationProp [] props;
    public AnimatorOverrideController [] controller;
    protected override void PostInit() {
        base.PostInit();

        for(int i = 0; i < SoldierTypes.Length; i++) {
            heads[i].SetActive(SoldierTypes[i] == npc.NPC);
            bodies[i].SetActive(SoldierTypes[i] == npc.NPC);
            if(SoldierTypes[i] == npc.NPC) {
                animator.ToggleProp(true, props[i]);
                animator.SetController(controller[i]);
            }

        }

        if(npc.NPC == NPCType.Proctor) {
            Entity.SetName("Proctor");
        } else {
            Entity.SetName("Militus");
        }
    }

}
