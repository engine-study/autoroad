using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public enum RoadState { None, Shoveled, Filled, Paved }

public class RoadComponent : MUDComponent {
    [Header("Road")]
    public RoadState state;
    public GameObject[] stages;
    public ParticleSystem fx_spawn, fx_fill;
    public AudioClip[] sfx_digs, sfx_fills;
    SPFlashShake flash;

    [Header("Debug")]
    public ChunkComponent parent;
    public int mileNumber;
    public Vector2Int chunkPos;
    RoadState lastStage = RoadState.None;


    protected override void Awake() {
        base.Awake();
        state = RoadState.None;
    }

    protected override void PostInit() {
        base.PostInit();

        //BIG BIG BIG
        Entity.GetMUDComponent<PositionComponent>().SetLayer(-1);

        AddToChunk();
    }

    protected override IMudTable GetTable() {return new RoadTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {

        RoadTable roadUpdate = (RoadTable)update;

        SetStage((RoadState)roadUpdate.value);

        if (newInfo.UpdateSource == UpdateSource.Optimistic || (Loaded && lastStage != state)) {
            if (state == RoadState.Shoveled) {
                fx_spawn.Play();
                SPAudioSource.Play(transform.position, sfx_digs);
                
                if(flash == null) { flash = gameObject.AddComponent<SPFlashShake>();}
                flash.SetTarget(stages[(int)state]);

            } else if (state == RoadState.Filled) {
                // fx_fill.Play();
                // audio.PlaySound(sfx_fills);
            }

            SetStage((RoadState)roadUpdate.value);

        }

        lastStage = state;

    }

    public void SetStage(RoadState newState) {
        
        state = newState;

        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)state);
        }
    }


    public async UniTaskVoid AddToChunk() {

        //we need the roadconfig info and the road pieces to have spawned to load the chunk
        //lots of waiting

        while (TableManager.FindTable<ChunkComponent>().SpawnedComponents.Count < 1) {
            await UniTask.Delay(500);
        }

        while (RoadConfigComponent.Width == 0) {
            await UniTask.Delay(500);
        }

        //infer mileNumber;
        mileNumber = Mathf.FloorToInt(transform.position.z / (float)RoadConfigComponent.Height);

        //infer entity of our chunk and look for it
        string chunkEntity = MUDHelper.Keccak256("Chunk", mileNumber);
        parent = TableManager.FindComponent<ChunkComponent>(chunkEntity);
        chunkPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);


        parent.AddRoadComponent(Entity.Key, this, chunkPos.x, chunkPos.y);
    }

}
