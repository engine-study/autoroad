using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public class TreeComponent : MUDComponent {

    [Header("Tree")]

    public bool treeState;
    public SPFlashShake flash;
    public ParticleSystem fx_hit, fx_fall;
    public AudioClip[] sfx_hits, sfx_falls;
    bool lastState = false;
    int lastHealth = -1;

    HealthComponent health;
    protected override void PostInit() {
        base.PostInit();
        health = Entity.GetMUDComponent<HealthComponent>();
        health.OnUpdated += TreeHit;
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        health.OnUpdated -= TreeHit;
    }

    void TreeHit() {

        if (health.UpdateSource != UpdateSource.Revert && Loaded && lastHealth != health.health) {


            if (health.health == 0 && health.UpdateSource == UpdateSource.Onchain) {
                SPAudioSource.Play(transform.position, sfx_falls);
                fx_fall.Play();
                flash.Flash();
            } else {
                SPAudioSource.Play(transform.position, sfx_hits);
                fx_hit.Play();
                flash.Flash();
            }
        }

        lastHealth = health.health;
    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {

        TreeTable treeUpdate = (TreeTable)update;

        if (treeUpdate == null) {
            Debug.LogError("No rockUpdate", this);
        } else {

            // stage = rockUpdate.rockType != null ? (int)rockUpdate.rockType : stage;
            // rockType = rockUpdate.rockType != null ? (RockType)rockUpdate.rockType : rockType;
            // Debug.Log(rockUpdate.value.ToString());

            treeState = treeUpdate.value != null ? (bool)treeUpdate.value : false;

        }

        lastState = treeState;

    }

    public void Chop() {
        ChopTree(Entity.Key);
    }

    public async void ChopTree(string entity) {
        await TxManager.Send<ChopFunction>(TxManager.MakeOptimistic(health, (int)Mathf.Clamp(health.health - 1, 0, Mathf.Infinity)), System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }
}
