using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

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

    protected override IMudTable GetTable() {return new RoadTable();}
    protected override void UpdateComponent(mud.IMudTable update, UpdateInfo newInfo) {

        RoadTable roadUpdate = (RoadTable)update;
        // Debug.Log("Road: " + newInfo.UpdateType.ToString() + " , " + newInfo.UpdateSource.ToString(), this);

        creditedPlayer = (string)roadUpdate.Filled;
        hasGem = (bool)roadUpdate.Gem;
        filledBy = MUDWorld.FindComponent<PlayerComponent>(creditedPlayer);
        creditedPlayerDebug = MUDWorld.MakeTable<RoadTable>(Entity.Key)?.Filled;

        SetState((RoadState)roadUpdate.State);

        if (newInfo.Source == UpdateSource.Optimistic || (Loaded && lastStage != State)) {

            flash = gameObject.GetComponent<SPFlashShake>() ?? gameObject.AddComponent<SPFlashShake>();
            flash.SetTarget(stages[(int)State]);
            
            if (State == RoadState.Ditch) {
                fx_spawn.Play();
                SPAudioSource.Play(transform.position, sfx_digs);
            }

            //only fire the big gun events if we're confirmed an onchain
            if(newInfo.Source == UpdateSource.Onchain) {
                if(State >= RoadState.Pavimentum) {
                    ToggleComplete(true);
                } else if(lastStage >= RoadState.Pavimentum) {
                    //somehow we reverted, this should not be possible
                    Debug.LogError("Not complete anymore", this);
                    ToggleComplete(false);
                }
            }
        }

        if(isFilled == false && (int)roadUpdate.State >= (int)RoadState.Pavimentum) {
            isFilled = true;
            CompletedRoadCount++;
            Debug.Log("Completed Road Count: " + CompletedRoadCount, this);
        }

        lastStage = State;

        if(Loaded && FilledBy) {
            OnCompletedRoad?.Invoke(this);
        }

    }

    
    Coroutine completeCoroutine = null;
    void ToggleComplete(bool toggle) {
        if(toggle) {
            completeCoroutine = StartCoroutine(SpawnCoins());
        } else {
            if(completeCoroutine != null) {
                StopCoroutine(completeCoroutine);
            }
        }
    }

    IEnumerator SpawnCoins() {
        yield return new WaitForSeconds(1f);
        // PlayerMUD player = EntityDictionary.FindEntity(creditedPlayer)?.GetMUDComponent<PlayerComponent>()?.GetComponent<PlayerMUD>();
        // if (player == null) {
        //     Debug.LogError("Couldn't find player " + creditedPlayer, this);
        //     yield break;
        // }
        SetComplete();
    }

    public void SetState(RoadState newState) {
        
        state = newState;

        if(Loaded) {
            for (int i = 0; i < stages.Length; i++) {
                stages[i].SetActive((i == (int)State && i < (int)RoadState.Pavimentum) || (i == (int)RoadState.Ditch && State >= RoadState.Pavimentum));
            }
        } else  {
            for (int i = 0; i < stages.Length; i++) {
                stages[i].SetActive(i == (int)State);
            }
        }

        Entity.SetName(state.ToString());

    }

    public void SetComplete() {
        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)State);
        }
    }

}
