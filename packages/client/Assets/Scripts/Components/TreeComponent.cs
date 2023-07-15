using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class TreeComponent : MUDComponent
{
    
    [Header("Tree")]

    public bool treeState;
    public ParticleSystem fx_hit, fx_fall;
    public AudioClip [] sfx_hits, sfx_falls;

    bool lastState = false;
    
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType) {

        base.UpdateComponent(update, eventType);

        TreeTable treeUpdate = (TreeTable)update;

        if (treeUpdate == null) {
            Debug.LogError("No rockUpdate", this);
        } else {

            // stage = rockUpdate.rockType != null ? (int)rockUpdate.rockType : stage;
            // rockType = rockUpdate.rockType != null ? (RockType)rockUpdate.rockType : rockType;
            // Debug.Log(rockUpdate.value.ToString());

            treeState = treeUpdate.value != null ? (bool)treeUpdate.value : false;

        }


        if (Loaded && lastState != treeState) {

            // if (eventType == UpdateEvent.Update || eventType == UpdateEvent.Optimistic) {
            //     source.PlaySound(sfx_whoosh);
            //     if(lastStage < RockType.Rudus) {
            //         source.PlaySound(sfx_pickHit);
            //         source.PlaySound(sfx_bigBreaks);
            //     } else {
            //         source.PlaySound(sfx_smallBreaks);
            //     }
              
            //     fx_break.Play();
            // }
        }

        lastState = treeState;

    }

    public void Chop() {
        ChopTree(Entity.Key);
    }

    public async void ChopTree(string entity) {
        // List<TxUpdate> updates = new List<TxUpdate>();
        // updates.Add(TxManager.MakeOptimistic(this, (Mathf.Clamp((int)rockType + 1, 0, (int)RockType.Rudus) )));
        // await TxManager.Send<MineFunction>(updates, x, y);
    }
}
