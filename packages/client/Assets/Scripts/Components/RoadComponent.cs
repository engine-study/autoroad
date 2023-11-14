using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;
using System;

public enum RoadState { Path, Ditch , Statumen, Rudus, Nucleas, Pavimentum, Ossimentum }

public class RoadComponent : MUDComponent {

    public static int CompletedRoadCount;
    public static System.Action<RoadComponent> OnCompletedRoad;
    public RoadState State {get { return state; } }
    public PlayerComponent FilledBy {get { return filledBy; } }
    public int Mile {get { return mileNumber; } }
    public bool Gem {get { return hasGem; } }

    [Header("Road")]
    [SerializeField] RoadState state;
    [SerializeField] GameObject[] stages;

    [Header("FX")]
    [SerializeField] ParticleSystem fx_spawn;
    [SerializeField] ParticleSystem fx_fill;
    [SerializeField] AudioClip[] sfx_digs, sfx_fills;
    SPFlashShake flash;

    [Header("Debug")]
    [SerializeField] int mileNumber;
    [SerializeField] PositionSync mover;
    [SerializeField] string creditedPlayer;
    [SerializeField] string creditedPlayerDebug;
    [SerializeField] bool hasGem;
    [SerializeField] ChunkComponent chunk;
    [SerializeField] PositionComponent pos;
    [SerializeField] PlayerComponent filledBy;
    [SerializeField] Vector2Int localPos;

    RoadState lastStage = RoadState.Path;


    protected override void Awake() {
        base.Awake();
        state = RoadState.Path;
    }

    protected override void PostInit() {
        base.PostInit();
        AddToChunk();
    }

    public async UniTaskVoid AddToChunk() {

        //infer mileNumber;

        //directly find mile chunk entity
        // string chunkEntity = MUDHelper.Keccak256("Chunk", mileNumber);
        // parent = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);

        pos = Entity.GetMUDComponent<PositionComponent>();
        mileNumber = (int)PositionComponent.PositionToMile(pos.Pos);
        
        while (ChunkLoader.Chunks[mileNumber] == null) { await UniTask.Delay(150);}

        //add this road to the chunk
        localPos = new Vector2Int((int)pos.Pos.x, (int)pos.Pos.z - mileNumber * MapConfigComponent.Height);
        chunk = ChunkLoader.Chunks[mileNumber];
        chunk.Mile.AddRoadComponent(Entity.Key, this, localPos.x, localPos.y);

    }
    bool isFilled = false;

    protected override MUDTable GetTable() {return new RoadTable();}
    protected override void UpdateComponent(mud.MUDTable update, UpdateInfo newInfo) {

        RoadTable roadUpdate = (RoadTable)update;
        // Debug.Log("Road: " + newInfo.UpdateType.ToString() + " , " + newInfo.UpdateSource.ToString(), this);

        state = (RoadState)(int)roadUpdate.State;
        creditedPlayer = (string)roadUpdate.Filled;
        hasGem = (bool)roadUpdate.Gem;
        filledBy = MUDWorld.FindComponent<PlayerTable, PlayerComponent>(creditedPlayer);
        creditedPlayerDebug = MUDWorld.GetTable<RoadTable>(Entity.Key)?.Filled;

        if (Loaded) {
            
            if(State >= RoadState.Pavimentum && Entity.gameObject.activeInHierarchy) {
                Entity.StartCoroutine(DelayStateCoroutine());
            } else if(lastStage != State) {
                UpdateState(true);
            }

        } else {
            UpdateState(false);
        }

        if(isFilled == false && (int)roadUpdate.State >= (int)RoadState.Pavimentum) {
            isFilled = true;
            CompletedRoadCount++;
            // Debug.Log("Completed Road Count: " + CompletedRoadCount, this);
        }

        lastStage = State;

    }

    IEnumerator DelayStateCoroutine() {

        // Debug.Log("Delay road fill", this);
        //wait for all positions to update
        yield return null;

        Vector3 underRoadPos = pos.Pos + Vector3.down * 2;
        mover = GridMUD.GetEntityAt(underRoadPos)?.GetRootComponent<PositionSync>();
        
        if(mover == null) {
            // Debug.Log("Couldn't find mover at " + underRoadPos, this);
            UpdateState(true); yield break;
        }

        while(mover.Moving) {yield return null;}

        // Debug.Log("Finished filling", this);
        UpdateState(true);
    }

    void UpdateState(bool withFX) {

        // Debug.Log($"Road new state {State}", this);

        for (int i = 0; i < stages.Length; i++) { stages[i].SetActive(i == (int)State);}
        Entity.SetName(state.ToString());

        if(withFX) {
            
            flash = gameObject.GetComponent<SPFlashShake>() ?? gameObject.AddComponent<SPFlashShake>();
            flash.SetTarget(stages[(int)State]);
            
            if (State == RoadState.Ditch) {
                fx_spawn.Play();
                SPAudioSource.Play(transform.position, sfx_digs);
            } else if(State >= RoadState.Pavimentum) {
                fx_fill.Play();
                SPAudioSource.Play(transform.position, sfx_fills);
            }
        }

        if(Loaded && FilledBy) {
            OnCompletedRoad?.Invoke(this);
        }
    }

}
