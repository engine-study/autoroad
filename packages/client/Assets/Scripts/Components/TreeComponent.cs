using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using DefaultNamespace;
using IWorld.ContractDefinition;

public enum FloraType {None, Tree, Oak, Bramble}
public class TreeComponent : MUDComponent {

    [Header("Tree")]
    public GameObject treeRoot;
    public GameObject [] types;
    public GameObject [] oakStages;
    public SPFlashShake flash;
    public ParticleSystem fx_hit, fx_branchFall;
    public AudioClip[] sfx_hits, sfx_falls;
    HealthComponent health;
    PositionComponent pos;
    Rigidbody rb;

    [Header("Debug")]
    public FloraType treeState;
    FloraType lastState = FloraType.None;
    int lastHealth = -999;
    protected override void PostInit() {
        base.PostInit();

        pos = Entity.GetMUDComponent<PositionComponent>();
        health = Entity.GetMUDComponent<HealthComponent>();

        health.OnUpdated += TreeHit;

    }

    protected override void InitDestroy() {
        base.InitDestroy();

        if(health) health.OnUpdated -= TreeHit;
    }
    
    protected override IMudTable GetTable() {return new TreeTable();}
    protected override void UpdateComponent(mud.IMudTable update, UpdateInfo newInfo) {

        TreeTable treeUpdate = (TreeTable)update;

        treeState = treeUpdate.value != null ? (FloraType)(int)treeUpdate.value : treeState;

        // if(newInfo.UpdateType == UpdateType.DeleteRecord) {
        //     gameObject.SetActive(false);
        // }

        lastState = treeState;

        Entity.SetName(treeState.ToString());
        
        for(int i = 0; i < types.Length; i++) {
            types[i].SetActive(i == (int)treeState);
        }

        flash.SetTarget(types[(int)treeState]);
    }

    protected override void UpdateComponentInstant() { 

    }
    protected override void UpdateComponentRich() { 
        if(UpdateInfo.UpdateType == UpdateType.SetRecord) {

        } else if(UpdateInfo.UpdateType == UpdateType.DeleteRecord) {

        }
    }

    void TreeHealthInstant() {
        for(int i = 0; i < oakStages.Length; i++) {
            oakStages[i].SetActive(i < health.Health);
        }
    }


    void TreeHit() {

        if ( Loaded && health.UpdateInfo.Source != UpdateSource.Revert && lastHealth != health.Health) {

            SPAudioSource.Play(transform.position, sfx_hits);
            fx_hit.Play();

            flash.SetTarget(types[(int)treeState]);
            flash.Flash();

            if(treeState == FloraType.Oak) {
                TreeHealthInstant();
                fx_branchFall.Play();
            } else {
                
            }
            
        }


        lastHealth = health.Health;
    }


    Coroutine fallCoroutine;
    IEnumerator FallCoroutine() {

        // if (rb == null) {
        //     rb = treeRoot.AddComponent<Rigidbody>();
        //     rb.drag = .5f;
        //     rb.centerOfMass = Vector3.zero;
        //     rb.angularVelocity = (Vector3.right * Random.Range(-5f,5f) + Vector3.forward * Random.Range(-5f,5f) );
        // }
        treeRoot.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        Entity.Toggle(false);
    }

    public void Chop() {
        ChopTree(Entity);
    }

    public async void ChopTree(mud.MUDEntity entity) {
        List<TxUpdate> updates = new List<TxUpdate>();
        
        updates.Add(TxManager.MakeOptimistic(health, health.Health - 1));

        if(health.Health - 1 <= 0) {
            updates.Add(TxManager.MakeOptimisticDelete(pos) );
        }

        await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, ActionName.Chop, transform.position, updates);
    }
}
