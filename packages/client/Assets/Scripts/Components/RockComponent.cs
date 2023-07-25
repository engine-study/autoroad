using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using mud.Unity;
using IWorld.ContractDefinition;

public enum RockType { None, Stone, Statumen, Pavimentum, Rudus, Nucleus, _Count }
public class RockComponent : MUDComponent {

    [Header("Rock")]
    [SerializeField] protected RockType rockType;
    [SerializeField] ParticleSystem fx_break, fx_drag, fx_fillParticles, fx_fillExplosion;
    [SerializeField] SPAudioSource source, rockSlide;
    [SerializeField] PositionSync posSync;

    [SerializeField] GameObject visualParent;
    [SerializeField] SPBase rockBase;
    [SerializeField] SPFlashShake flash;
    [SerializeField] GameObject[] stages;
    public AudioClip[] sfx_drag, sfx_dragBase, sfx_smallBreaks, sfx_bigBreaks, sfx_fillSound, sfx_finalThump;
    RockType lastStage = RockType._Count;

    protected override void Awake() {
        base.Awake();

        // Debug.Log("Rock Awake", this);
        rockType = RockType._Count;
        rockBase = GetComponent<SPBase>();

    }

    protected override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        Entity.SetName(rockType.ToString());

    }

    protected override void PostInit() {
        base.PostInit();

        posSync.Pos.OnUpdatedInfo += UpdatePositionCheck;
        // posSync.OnMoveComplete += CheckSink;

        if (posSync.Pos.NetworkInfo.UpdateType == UpdateType.DeleteRecord) {
            gameObject.SetActive(false);
        }
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        posSync.Pos.OnUpdatedInfo -= UpdatePositionCheck;
        // posSync.OnMoveComplete -= CheckSink;
    }


    Vector3 lastPos;
    void UpdatePositionCheck(MUDComponent c, UpdateInfo newInfo) {

        PositionComponent pos = c as PositionComponent;

        if (Loaded) {

            if (newInfo.UpdateSource != UpdateSource.Revert && lastPos != pos.Pos) {
                fx_drag.Play();
                source.PlaySound(sfx_drag);
                source.PlaySound(sfx_dragBase);
                flash.Flash();
            }

            //our position component was deleted
            //we got pushed into a hole, when we finish moving to the hole, sink into into 
            if (newInfo.UpdateType == UpdateType.DeleteRecord) {
                CheckSink();
            }
        }

        lastPos = pos.Pos;

    }

    void CheckSink() {

        Debug.Log("Check Sink", this);

        // if (!Loaded) {
        //     return;
        // }

        //we stopped moving, AND we have a deleleted record, lets get pushed into a hole
        if (posSync.Pos.NetworkInfo.UpdateType == UpdateType.DeleteRecord) {
            Sink();
        }
    }

    Coroutine sinkCoroutine;
    void Sink() {

        if (sinkCoroutine != null) {
            StopCoroutine(sinkCoroutine);
        }

        StartCoroutine(SinkCoroutine());

    }

    IEnumerator SinkCoroutine() {

        Debug.Log("Sinking", this);

        while (posSync.Moving) { yield return null; }
        if (posSync.Pos.NetworkInfo.UpdateType != UpdateType.DeleteRecord) {
            Debug.LogError("Sunk but we aren't deleleted");
            yield break;
        }

        flash.Flash();

        fx_fillParticles.Play();
        source.PlaySound(sfx_fillSound);

        rockSlide.Source.Play();
        rockSlide.Source.time = Random.Range(0f, rockSlide.Source.clip.length);

        float lerp = 0f;

        while (lerp < 1f) {
            lerp += Time.deltaTime * .66f;
            visualParent.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down * 1f, lerp);
            yield return null;
        }

        rockSlide.Source.Stop();
        fx_fillParticles.Emit(10);
        fx_fillExplosion.Play();
        source.PlaySound(sfx_finalThump);

        visualParent.SetActive(false);

        SPCamera.AddShake( Mathf.Clamp01(1f - Vector3.Distance(transform.position, SPPlayer.LocalPlayer.Root.position) * .1f) * .25f);
    }


    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {

        RockTable rockUpdate = (RockTable)update;

        if (rockUpdate == null) {
            Debug.LogError("No rockUpdate", this);
        } else {
            // rockType = rockUpdate.value != null ? (RockType)rockUpdate.value : RockType._Count;
            rockType = (RockType)rockUpdate.value;
        }

        // Debug.Log(rockType.ToString());

        rockBase.baseName = rockType.ToString();
        Entity.SetName(rockType.ToString());

        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)rockType);
        }

        flash.SetTarget(stages[(int)rockType].GetComponent<RandomSelector>()?.ActiveChild);

        if (Loaded && lastStage != rockType) {

            if (newInfo.UpdateType == UpdateType.SetField || newInfo.UpdateSource == UpdateSource.Optimistic) {
                if (lastStage < RockType.Rudus) {
                    source.PlaySound(sfx_bigBreaks);
                } else {
                    source.PlaySound(sfx_smallBreaks);
                }

                flash.Flash();
                fx_break.Play();
            }
        }

        lastStage = rockType;

    }

    public void Mine() {
        MineRock((int)transform.position.x, (int)transform.position.z);
    }

    public async void MineRock(int x, int y) {
        await TxManager.Send<MineFunction>(TxManager.MakeOptimistic(this, Mathf.Clamp((int)rockType + 1, 0, (int)RockType.Rudus)), x, y);
    }

}
