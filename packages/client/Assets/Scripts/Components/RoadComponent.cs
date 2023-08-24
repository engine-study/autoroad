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
    [SerializeField] ChunkComponent parent;
    [SerializeField] PlayerComponent filledBy;
    [SerializeField] Vector2Int localChunkPos;

    RoadState lastStage = RoadState.None;


    protected override void Awake() {
        base.Awake();
        state = RoadState.None;
    }

    protected override void PostInit() {
        base.PostInit();
        AddToChunk();
    }

    protected override IMudTable GetTable() {return new RoadTable();}
    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateInfo newInfo) {


        RoadTable roadUpdate = (RoadTable)update;
        // Debug.Log("Road: " + newInfo.UpdateType.ToString() + " , " + newInfo.UpdateSource.ToString(), this);

        creditedPlayer = ((string)roadUpdate.filled).ToLower();
        hasGem = (bool)roadUpdate.gem;

        filledBy = MUDWorld.FindComponent<PlayerComponent>(creditedPlayer);

        SetState((RoadState)roadUpdate.state);

        if (newInfo.UpdateSource == UpdateSource.Optimistic || (Loaded && lastStage != State)) {

            flash = gameObject.GetComponent<SPFlashShake>() ?? gameObject.AddComponent<SPFlashShake>();
            flash.SetTarget(stages[(int)State]);
            
            if (State == RoadState.Shoveled) {
                fx_spawn.Play();
                SPAudioSource.Play(transform.position, sfx_digs);
            }

            //only fire the big gun events if we're confirmed an onchain
            if(newInfo.UpdateSource == UpdateSource.Onchain) {
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
    
    Coroutine coinsCoroutine = null;
    void ToggleComplete(bool toggle) {
        if(toggle) {
            coinsCoroutine = StartCoroutine(SpawnCoins());
        } else {
            if(coinsCoroutine != null) {
                StopCoroutine(coinsCoroutine);
            }
        }
    }

    IEnumerator SpawnCoins() {

        yield return new WaitForSeconds(1f);

        SetComplete();

        yield return new WaitForSeconds(1f);

        PlayerMUD player = EntityDictionary.FindEntity(creditedPlayer)?.GetMUDComponent<PlayerComponent>()?.GetComponent<PlayerMUD>();

        if (player == null) {
            Debug.LogError("Couldn't find player", this);
            yield break;
        }


        for (int i = 0; i < 5; i++) {
            SPResourceJuicy coin = SPResourceJuicy.SpawnResource("Prefabs/Coin", player.Root, transform.position + Vector3.up, Random.rotation);
            coin.SendResource();
            yield return new WaitForSeconds(.1f);
        }

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

    public async UniTaskVoid AddToChunk() {

        //infer mileNumber;
        mileNumber = (int)WorldScroll.PositionToMile(transform.position);

        //load mile chunk
        string chunkEntity = MUDHelper.Keccak256("Chunk", mileNumber);
        parent = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);
        
        while (parent == null) {
            await UniTask.Delay(500);
            parent = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);
        }

        //infer entity of our chunk and look for it
        localChunkPos = new Vector2Int((int)transform.position.x, (int)transform.position.z - mileNumber * RoadConfigComponent.Height);

        parent.AddRoadComponent(Entity.Key, this, localChunkPos.x, localChunkPos.y);
    }

}
