using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public enum RoadState { None,Shoveled,Statumen,Rudus,Nucleas,Paved, Bones }

public class RoadComponent : MUDComponent {

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

    RoadState lastStage = RoadState.None;


    protected override void Awake() {
        base.Awake();
        state = RoadState.None;
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

    protected override IMudTable GetTable() {return new RoadTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {


        RoadTable roadUpdate = (RoadTable)update;
        // Debug.Log("Road: " + newInfo.UpdateType.ToString() + " , " + newInfo.UpdateSource.ToString(), this);

        creditedPlayer = ((string)roadUpdate.filled).ToLower();
        hasGem = (bool)roadUpdate.gem;

        filledBy = MUDWorld.FindComponent<PlayerComponent>(creditedPlayer);

        SetState((RoadState)roadUpdate.state);

        if (newInfo.Source == UpdateSource.Optimistic || (Loaded && lastStage != State)) {

            flash = gameObject.GetComponent<SPFlashShake>() ?? gameObject.AddComponent<SPFlashShake>();
            flash.SetTarget(stages[(int)State]);
            
            if (State == RoadState.Shoveled) {
                fx_spawn.Play();
                SPAudioSource.Play(transform.position, sfx_digs);
            }

            //only fire the big gun events if we're confirmed an onchain
            if(newInfo.Source == UpdateSource.Onchain) {
                if(State >= RoadState.Paved) {
                    ToggleComplete(true);
                } else if(lastStage >= RoadState.Paved) {
                    //somehow we reverted, this should not be possible
                    Debug.LogError("Not complete anymore", this);
                    ToggleComplete(false);
                }
            }


        }

        creditedPlayerDebug = MUDWorld.FindValue<RoadTable>(Entity.Key).filled;

        lastStage = State;

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
                stages[i].SetActive((i == (int)State && i < (int)RoadState.Paved) || (i == (int)RoadState.Shoveled && State >= RoadState.Paved));
            }
        } else  {
            for (int i = 0; i < stages.Length; i++) {
                stages[i].SetActive(i == (int)State);
            }
        }

        Entity.SetName(newState == RoadState.Shoveled ? "Ditch" : "Road");

    }

    public void SetComplete() {
        for (int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == (int)State);
        }
    }

}
