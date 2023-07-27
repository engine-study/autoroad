using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
public class TreeComponent : MUDComponent {

    [Header("Tree")]
    public GameObject treeRoot;
    public SPFlashShake flash;
    public ParticleSystem fx_hit, fx_fall;
    public AudioClip[] sfx_hits, sfx_falls;
    HealthComponent health;
    Rigidbody rb;

    [Header("Debug")]
    public bool treeState;
    bool lastState = false;
    int lastHealth = -1;

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

            if (health.health == 0) {
                SPAudioSource.Play(transform.position, sfx_hits);
                SPAudioSource.Play(transform.position, sfx_falls);
                fx_hit.Play();
                fx_fall.Play();
                flash.Flash();

                fallCoroutine = StartCoroutine(FallCoroutine());

            } else {
                SPAudioSource.Play(transform.position, sfx_hits);
                fx_hit.Play();
                flash.Flash();
            }
        } else if (health.UpdateSource == UpdateSource.Revert) {

            if (fallCoroutine != null) {
                StopCoroutine(fallCoroutine);
            }

            if (rb) {
                rb.isKinematic = true;
            }

            treeRoot.transform.localPosition = Vector3.zero;
            treeRoot.transform.localRotation = Quaternion.identity;
        }

        lastHealth = health.health;
    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {

        TreeTable treeUpdate = (TreeTable)update;

        treeState = treeUpdate.value != null ? (bool)treeUpdate.value : false;

        // if(newInfo.UpdateType == UpdateType.DeleteRecord) {
        //     gameObject.SetActive(false);
        // }

        lastState = treeState;

    }

    Coroutine fallCoroutine;
    IEnumerator FallCoroutine() {
        if (rb == null)
            rb = treeRoot.AddComponent<Rigidbody>();
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }

    public void Chop() {
        ChopTree(Entity.Key);
    }

    public async void ChopTree(string entity) {
        await TxManager.Send<ChopFunction>(TxManager.MakeOptimistic(health, (int)Mathf.Clamp(health.health - 1, 0, Mathf.Infinity)), System.Convert.ToInt32(transform.position.x), System.Convert.ToInt32(transform.position.z));
    }
}
