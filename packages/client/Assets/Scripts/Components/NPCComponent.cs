using System.Collections;
using System.Collections.Generic;
using mudworld;
using mud;
using UnityEngine;

public class NPCComponent : MUDComponent
{
    public NPCType NPC {get{return npc;}}

    [Header("NPC")]
    [SerializeField] NPCKit [] kits;

    [Header("Debug")]
    [SerializeField] NPCType npc;
    [SerializeField] ActionComponent action;
    ActionName lastAction = ActionName.None;

    protected override void PostInit() {
        base.PostInit();

        // SetKit(kits[(int)npc]);
        action = Entity.GetMUDComponent<ActionComponent>();
        action.OnUpdated += CheckDead;

    }

    protected override MUDTable GetTable() {return new NPCTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        NPCTable table = update as NPCTable;
        npc = (NPCType)(int)table.Value;
     
    }

    void CheckDead() {
        
        //we died somehow
        if(action.Action == ActionName.Dead && lastAction != action.Action) {
            if(NPC == NPCType.Player) {
                NotificationUI.AddNotification($"{Entity.Name} was smote.");
            } else {
                NotificationUI.AddNotification($"An NPC {Entity.Name} perished.");
            }
        }

        lastAction = action.Action;
        
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
