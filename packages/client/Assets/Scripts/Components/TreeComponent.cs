using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;

public enum FloraType {None, Tree, Oak, Bramble}
public class TreeComponent : MUDComponent {

    [Header("Tree")]
    public GameObject treeRoot;
    public GameObject [] types;
    public SPFlashShake flash;
    public ParticleSystem fx_hit, fx_fall;
    public AudioClip[] sfx_hits, sfx_falls;
    HealthComponent health;
    PositionComponent pos;
    Rigidbody rb;

    [Header("Debug")]
    public FloraType treeState;
    FloraType lastState = FloraType.None;
    int lastHealth = -999;
    UpdateType lastPosUpdateType;
    protected override void PostInit() {
        base.PostInit();

        pos = Entity.GetMUDComponent<PositionComponent>();
        health = Entity.GetMUDComponent<HealthComponent>();

        pos.OnInstantUpdate += TreeVisibility;
        pos.OnRichUpdate += TreeSpawnAnimation;

        health.OnUpdated += TreeHit;

    }

    protected override void InitDestroy() {
        base.InitDestroy();

        if(health) health.OnUpdated -= TreeHit;
        if(pos) pos.OnUpdated -= TreeSpawnAnimation;
    }
    
    protected override IMudTable GetTable() {return new TreeTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {

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
    }

    protected override void UpdateComponentInstant() { 

    }
    protected override void UpdateComponentRich() { 
        if(UpdateInfo.UpdateType == UpdateType.SetRecord) {

        } else if(UpdateInfo.UpdateType == UpdateType.DeleteRecord) {

        }
    }

    void TreeVisibility() {
        gameObject.SetActive(pos.UpdateInfo.UpdateType != UpdateType.DeleteRecord);
    }

    void TreeSpawnAnimation() {

        gameObject.SetActive(pos.UpdateInfo.UpdateType != UpdateType.DeleteRecord);

        // if (health.UpdateSource != UpdateSource.Revert && pos.UpdateType == UpdateType.DeleteRecord && Loaded && pos.UpdateType != lastPosUpdateType) {
        //     SPAudioSource.Play(transform.position, sfx_hits);
        //     SPAudioSource.Play(transform.position, sfx_falls);
        //     fx_fall.Play(true);
        //     flash.Flash();
        //     fallCoroutine = StartCoroutine(FallCoroutine());

        // } else {

        //     //if we reverted to an alive state, fix
        //     if (fallCoroutine != null) {
        //         StopCoroutine(fallCoroutine);
        //     }

        //     if (rb) {
        //         rb.isKinematic = true;
        //     }

        //     treeRoot.transform.localPosition = Vector3.zero;
        //     treeRoot.transform.localRotation = Quaternion.identity;
            
        // }

        lastPosUpdateType = pos.UpdateInfo.UpdateType;

    }

    void TreeHit() {

        if (health.UpdateInfo.Source != UpdateSource.Revert && Loaded && lastHealth != health.Health) {

            SPAudioSource.Play(transform.position, sfx_hits);
            fx_hit.Play();
            flash.Flash();
            
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
        treeRoot.gameObject.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    public void Chop() {
        ChopTree(Entity);
    }

    public async void ChopTree(MUDEntity entity) {
        List<TxUpdate> updates = new List<TxUpdate>();
        
        updates.Add(TxManager.MakeOptimistic(health, (int)Mathf.Clamp(health.Health - 1, 0, Mathf.Infinity)));
        updates.Add(TxManager.MakeOptimisticDelete(pos) );

        await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity, ActionName.Chop, transform.position, updates);
    }
}
