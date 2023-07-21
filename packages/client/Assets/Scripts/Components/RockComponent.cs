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
    [SerializeField] ParticleSystem fx_break, fx_drag, fx_fillParticles;
    [SerializeField] SPAudioSource source, rockSlide;
    [SerializeField] PositionSync posSync;

    [SerializeField] GameObject visualParent;
    [SerializeField] SPBase rockBase;
    [SerializeField] GameObject[] stages;
    public AudioClip[] sfx_drag, sfx_dragBase, sfx_pickHit, sfx_whoosh, sfx_smallBreaks, sfx_bigBreaks, sfx_fillSound, sfx_finalThump;
    RockType lastStage = RockType._Count;

    protected override void Awake() {
        base.Awake();

        // Debug.Log("Rock Awake", this);
        rockType = RockType._Count;
        rockBase = GetComponent<SPBase>();
    }

    public override void Init(MUDEntity ourEntity, TableManager ourTable) {
        base.Init(ourEntity, ourTable);

        ourEntity.OnComponentUpdated += UpdatePositionCheck;
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        Entity.OnComponentUpdated -= UpdatePositionCheck;
        posSync.OnMoveComplete -= Sink;
        

    }


    protected override void PostInit() {
        base.PostInit();

        if(posSync.Pos.UpdateType == UpdateType.DeleteRecord) {
            gameObject.SetActive(false);
        }
    }

    Vector3 lastPos;
    void UpdatePositionCheck(MUDComponent c, UpdateInfo newInfo) {

        PositionComponent pos = c as PositionComponent;
        if (pos) {

            if(Loaded) {
                if (newInfo.UpdateSource != UpdateSource.Revert && lastPos != pos.Pos) {
                    fx_drag.Play();
                    source.PlaySound(sfx_drag);
                    source.PlaySound(sfx_dragBase);
                }
            }

            //our position component was deleted
            //we got pushed into a hole, when we finish moving to the hole, sink into into it
            if(newInfo.UpdateType == UpdateType.DeleteRecord) {
        
                if(Loaded) {
                    if(posSync.Moving) posSync.OnMoveComplete += Sink;
                    else Sink();
                } else {
                    gameObject.SetActive(false);
                }
            }

            lastPos = pos.Pos;
        }

    }

    void Sink() {

        StartCoroutine(SinkCoroutine());

    }

    IEnumerator SinkCoroutine() {

        Debug.Log("Sinking", this);

        fx_fillParticles.Play();
        source.PlaySound(sfx_fillSound);

        rockSlide.Source.Play();
        rockSlide.Source.time = Random.Range(0f, rockSlide.Source.clip.length);

        float lerp = 0f;

        while(lerp < 1f) {
            lerp += Time.deltaTime * .5f;
            visualParent.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down * .2f, lerp);
            yield return null;
        }

        source.PlaySound(sfx_finalThump);
        rockSlide.Source.Stop();
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
        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)rockType);
        }

        if (Loaded && lastStage != rockType) {

            if (newInfo.UpdateType == UpdateType.SetField || newInfo.UpdateSource == UpdateSource.Optimistic) {
                source.PlaySound(sfx_whoosh);
                if (lastStage < RockType.Rudus) {
                    source.PlaySound(sfx_pickHit);
                    source.PlaySound(sfx_bigBreaks);
                } else {
                    source.PlaySound(sfx_smallBreaks);
                }

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
