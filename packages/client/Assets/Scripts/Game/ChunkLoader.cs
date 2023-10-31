using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;

public class ChunkLoader : MonoBehaviour
{
    public static System.Action OnActiveChunk;
    public static System.Action<ChunkComponent> OnChunkRegistered;

    public static bool HasLoadedAllChunks{get{return ChunkList != null && ChunkList.Count == GameStateComponent.MILE_COUNT+1 && GameStateComponent.MILE_COUNT > -1;}}
    public static ChunkLoader Instance;
    public static ChunkComponent ActiveChunk;
    public static ChunkComponent GetChunk(int mile) {Chunks.TryGetValue(mile, out var chunk); return chunk; }
    public static Dictionary<int, ChunkComponent> Chunks;
    public static Dictionary<PositionComponent, int> PositionsOnMiles;
    public static List<ChunkComponent> ChunkList;
    public static int Mile {get{return Instance.mile;}}
    public static ChunkComponent Chunk {get{return Instance.chunk;}}

    [Header("Chunks")]
    [SerializeField] TableManager pos;

    [Header("Debug")]
    [SerializeField] int mile;
    [SerializeField] ChunkComponent chunk;

    void Start() {
        Instance = this;

        Chunks = new Dictionary<int, ChunkComponent>();
        ChunkList = new List<ChunkComponent>();
        PositionsOnMiles = new Dictionary<PositionComponent, int>();

        pos.OnComponentSpawned += PositionsToChunk;

        Debug.Log("[CHUNK] STATIC INIT");
    }

    void OnDestroy() {
        Instance = null;
             
        Chunks = null;
        ChunkList = null;
        PositionsOnMiles = null;

        // TableManager pos = MUDWorld.FindTable<PositionComponent>();
        // if(pos) pos.OnComponentSpawned -= PositionsToChunk;
    }

    public static void RegisterChunk(ChunkComponent newChunk) {
        if(Chunks.ContainsKey(newChunk.MileNumber)) { Debug.LogError("[CHUNK] " + newChunk.MileNumber + " already exists", newChunk); return;}
        ChunkList.Add(newChunk);
        Chunks.Add(newChunk.MileNumber, newChunk);

        if (newChunk.MileNumber == GameStateComponent.MILE_COUNT) { 
            ActiveChunk = newChunk; 
            OnActiveChunk?.Invoke();
        }

        Debug.Log("[CHUNK] " + newChunk.MileNumber + " Completed: " + newChunk.Completed.ToString() + " Spawned: " + newChunk.Spawned.ToString() + " TOTAL: " + ChunkList.Count, newChunk);

        OnChunkRegistered?.Invoke(newChunk);
    }

    public static bool LoadMile(int newMile) {
        return Instance.LoadMileInternal(newMile);
    }

    public bool LoadMileInternal(int newMile) {
        
        // Debug.Log("[CHUNK]: Loading " + newMile, this);

        // string chunkEntity = MUDHelper.Keccak256("Chunk", (int)newMile);
        // ChunkComponent newChunk = MUDWorld.FindComponent<ChunkComponent>(chunkEntity);

        ChunkLoader.Chunks.TryGetValue(newMile, out ChunkComponent newChunk);

        if(newChunk == null) {
            Debug.LogError("Couldn't load mile " + newMile, this);
            return false; 
        }

        if(chunk && chunk != newChunk) {
            bool isCloseEnough = Mathf.Abs(chunk.MileNumber - newMile) < 2;
            chunk.Toggle(isCloseEnough);
        }

        for(int i = 0; i < ChunkList.Count; i++) {
            bool showChunk = i == newMile || (i >= newMile-1 && i <= newMile+1);
            ChunkList[i].Toggle(showChunk);
        }

        chunk = newChunk;        
        return true;
    }

    static void PositionsToChunk(MUDComponent newPos) {
        PositionComponent pos = (PositionComponent)newPos;
        MUDEntity entity = newPos.Entity;

        if(PositionsOnMiles.ContainsKey(pos)) {Debug.LogError("Double spawn"); return;}

        // Debug.Log("CHUNK: " + newPos.Entity.gameObject.name, newPos.Entity);
        PositionsOnMiles[pos] = -1;

        if(newPos.Entity.Loaded) { PositionMileSpawn(newPos.Entity);}
        else {entity.OnLoadedInfo += PositionMileSpawn;}
        pos.OnUpdatedInfo += PositionMileUpdate;
    }

    static void PositionMileSpawn(MUDEntity entity) {
        AddEntityToChunk(entity.GetMUDComponent<PositionComponent>());
    }

    static void PositionMileUpdate(MUDComponent c, UpdateInfo update) {
        if(c.Entity.Loaded == false) return;
        AddEntityToChunk((PositionComponent)c);
    }

    static void AddEntityToChunk(PositionComponent pos) {

        int mile = (int)PositionComponent.PositionToMile(pos.Pos);
        int onMile = PositionsOnMiles[pos];

        bool unparent = false;
        //remove from old mile or we're still on the same mile
        if(onMile != -1 && onMile != mile && GetChunk(onMile) != null) {
            GetChunk(onMile).Positions.Remove(pos);
            unparent = true;
        }

        //add to mile
        ChunkComponent chunk = GetChunk(mile);
        if(chunk == null) {

        } else {
            //parent
            chunk.Positions.Add(pos);
            pos.Entity.transform.parent = chunk.Objects;
            unparent = false;
        }

        //add to mile dictionary and list
        PositionsOnMiles[pos] = mile;

        if(unparent) {
            pos.Entity.transform.parent = EntityDictionary.Parent;
        }
    }
}
