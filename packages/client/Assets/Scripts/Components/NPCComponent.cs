using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using mud.Client;
using UnityEngine;

public enum NPCType {None, Player, Soldier, Barbarian, Ox}
public class NPCComponent : MUDComponent
{
    public NPCType NPC {get{return npc;}}

    [Header("NPC")]
    [SerializeField] NPCType npc;
    protected override IMudTable GetTable() {return new NPCTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        NPCTable table = update as NPCTable;
        npc = (NPCType)(int)table.value;
     
    }

}

