using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using mud;
using IWorld.ContractDefinition;

public enum RockType { None, Rock, Statumen, Pavimentum, Rudus, Nucleus, Miliarium, Lapis, Saxum, Columna, Statuae, _Count }
public class RockComponent : MUDComponent {

    public RockType RockType {get { return rockType; } }

    [Header("Rock")]
    [SerializeField] PositionSync posSync;
    [SerializeField] GameObject visualParent;
    [SerializeField] SPBase rockBase;
    [SerializeField] SPFlashShake flash;
    [EnumNamedArray( typeof(RockType) )]
    [SerializeField] GameObject[] stages;
    [SerializeField] ParticleSystem fx_break, fx_drag, fx_fillParticles, fx_fillExplosion;
    [SerializeField] AudioClip[] sfx_slide, sfx_drag, sfx_dragBase, sfx_smallBreaks, sfx_bigBreaks, sfx_fillSound, sfx_finalThump;
    RockType lastStage = RockType._Count;

    [Header("Debug")]
    [SerializeField] RockType rockType;
    [SerializeField] MoveComponent moveComponent;


    protected override void Awake() {
        base.Awake();

        // Debug.Log("Rock Awake", this);
        rockType = RockType._Count;
        rockBase = GetComponent<SPBase>();

    }


    protected override void Init(SpawnInfo newSpawnInfo) {
        base.Init(newSpawnInfo);

        Entity.SetName(rockType.ToString());
    }

    protected override void PostInit() {
        base.PostInit();

        moveComponent = Entity.GetMUDComponent<MoveComponent>();
        moveComponent.OnMove += MoveRock;
        moveComponent.OnHole += Sink;
    }

    protected override void InitDestroy() {
        base.InitDestroy();

        if(moveComponent) {
            moveComponent.OnMove -= MoveRock;
            moveComponent.OnHole -= Sink;
        }
    }

    protected override IMudTable GetTable() { return new RockTable(); }
    protected override void UpdateComponent(mud.IMudTable update, UpdateInfo newInfo) {

        RockTable rockUpdate = (RockTable)update;

        if (rockUpdate == null) { Debug.LogError("No rockUpdate", this);
        } else { rockType = (RockType)rockUpdate.Value;}

        rockBase.baseName = rockType.ToString();
        Entity.SetName(rockType.ToString());

        for (int i = 0; i < stages.Length; i++) {
            stages[i]?.SetActive(i == (int)rockType);
        }
        
        if(Loaded) {
            flash.SetTarget(stages[(int)rockType]);
        }

    }

    protected override void UpdateComponentRich() {
        base.UpdateComponentRich();

        flash.Flash();
        fx_break.Play();

        if (rockType < RockType.Rudus) {
            SPAudioSource.Play(transform.position, sfx_bigBreaks);
        } else {
            SPAudioSource.Play(transform.position, sfx_smallBreaks);
        }

        lastStage = rockType;

    }

    void MoveRock() {
        fx_drag.Play();
        flash.Flash();
        SPAudioSource.Play(transform.position,sfx_drag);
        SPAudioSource.Play(transform.position,sfx_dragBase);
    }

    Coroutine sinkCoroutine;
    void Sink() {

        if (sinkCoroutine != null) { StopCoroutine(sinkCoroutine); }
        StartCoroutine(SinkCoroutine());

    }

    IEnumerator SinkCoroutine() {

        Debug.Log("Sinking", this);

        while (posSync.Moving) { yield return null; }
        
        flash.Flash();

        fx_fillParticles.Play();

        SPAudioSource.Play(transform.position, sfx_slide);
        SPAudioSource.Play(transform.position, sfx_fillSound);

        float lerp = 0f;

        while (lerp < 1f) {
            lerp += Time.deltaTime * 2f;
            visualParent.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down * 1f, lerp);
            yield return null;
        }

        fx_fillParticles.Emit(10);
        fx_fillExplosion.Play();
        SPAudioSource.Play(transform.position, sfx_finalThump);

        visualParent.SetActive(false);

        SPCamera.AddShake(Mathf.Clamp01(1f - Vector3.Distance(transform.position, SPPlayer.LocalPlayer.Root.position) * .1f) * .1f, transform.position);
        RoadComponent road = MUDWorld.FindComponent<RoadTable, RoadComponent>(MUDHelper.Keccak256("Road", (int)posSync.Pos.Pos.x, (int)posSync.Pos.Pos.z));

        if(road == null) {
            Debug.LogError("Can't find road", this);
            yield break;
        }

    }

    public void Mine() {
        MineRock((int)transform.position.x, (int)transform.position.z);
    }

    public async void MineRock(int x, int y) {
        List<TxUpdate> updates = new List<TxUpdate>() { TxManager.MakeOptimistic(this, Mathf.Clamp((int)rockType + 1, 0, (int)RockType.Nucleus)) };
        await ActionsMUD.ActionTx(PlayerComponent.LocalPlayer.Entity,  ActionName.Mining, new Vector3(x, 0, y), updates);
    }

}
