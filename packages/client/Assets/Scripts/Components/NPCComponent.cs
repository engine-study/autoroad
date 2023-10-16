using System.Collections;
using System.Collections.Generic;
using mudworld;
using mud;
using UnityEngine;

public enum NPCType {None, Player, Soldier, Barbarian, Ox, BarbarianArcher}
public class NPCComponent : MUDComponent
{
    public NPCType NPC {get{return npc;}}

    [Header("NPC")]
    [SerializeField] NPCKit [] kits;

    [Header("Debug")]
    [SerializeField] NPCType npc;

    protected override void PostInit() {
        base.PostInit();

        // SetKit(kits[(int)npc]);

    }

    protected override IMudTable GetTable() {return new NPCTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        NPCTable table = update as NPCTable;
        npc = (NPCType)(int)table.Value;
     
    }

    public void SetKit(NPCKit newKit) {

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

[System.Serializable]
public class NPCKit{
    public SPAnimationProp prop;

}
