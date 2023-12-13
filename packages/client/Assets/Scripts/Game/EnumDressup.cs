using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class EnumDressup : MonoBehaviour
{


    [Header("Dressup")]
    public MoverNPCComponent mover; 
    public NPCType [] NPCTypesSupported = {NPCType.Soldier, NPCType.Proctor};
    public GameObject [] heads;
    public GameObject [] bodies;
    public SPAnimationProp [] props;
    public AnimatorOverrideController [] controller;

    public void Start() {
        
        for(int i = 0; i < NPCTypesSupported.Length; i++) {
            heads[i]?.SetActive(NPCTypesSupported[i] == mover.NPC.NPC);
            bodies[i]?.SetActive(NPCTypesSupported[i] == mover.NPC.NPC);
            if(NPCTypesSupported[i] == mover.NPC.NPC) {
                mover.Entity.SetName( mover.NPC.NPC.ToString());
                mover.Animator.ToggleProp(true, props[i]);
                if(controller[i] != null) mover.Animator.SetController(controller[i]);
            }

        }
    }
}
