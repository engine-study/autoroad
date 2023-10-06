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

    
    public void Meleed(bool toggle, IActor actor) {

        if(!toggle) { return;}

        NPCComponent otherPlayer = actor.Owner().GetComponent<NPCComponent>();

        if(otherPlayer == null) {
            Debug.LogError("Not sure: " + actor.Owner().name, this);
        }

        Debug.Log("Meleed", this);

        string targetAddress = otherPlayer.Entity.Key;
        // List<TxUpdate> update = new List<TxUpdate>() { TxManager.MakeOptimistic(health, health.Health == 1 ? -1 : health.Health - 1) };
        // ActionsMUD.ActionTx(update, ActionName.Melee, playerScript.Position.Pos);

    }


}

