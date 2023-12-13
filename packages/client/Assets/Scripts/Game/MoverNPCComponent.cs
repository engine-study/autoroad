using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverNPCComponent : MoverComponent
{
    public NPCComponent NPC {get{return npc;}}
    [Header("NPC")]
    public NPCComponent npc;

    protected override void PostInit() {
        base.PostInit();
        npc = Entity.GetMUDComponent<NPCComponent>();
    }
}
