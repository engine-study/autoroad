using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;

public class SoldierComponent : MoverNPCComponent
{
    [Header("Soldier")]
    public GameObject [] heads;
    public GameObject [] bodies;
    protected override void PostInit() {
        base.PostInit();

        if(npc.NPC == NPCType.Proctor) {
            Entity.SetName("Proctor");
        } else {
            Entity.SetName("Militus");
        }
    }

}
