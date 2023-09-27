using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class ChunkComponent : MUDComponent
{

    public static System.Action OnActiveChunk;
    public static System.Action OnChunkUpdate;
    
    public static ChunkComponent ActiveChunk;
    public static ChunkComponent GetChunk(int mile) {Chunks.TryGetValue(mile, out var chunk); return chunk; }
    public static Dictionary<int, ChunkComponent> Chunks;
    public static List<ChunkComponent> ChunkList;
    public static Dictionary<PositionComponent, int> PositionsOnMiles;
    public Transform Objects {get{return entityParent;}}

    public Mile Mile {get{return mile;}}
    public int MileNumber {get{return mileNumber;}}
    public bool Completed {get{return completed;}}
    public bool Spawned {get{return spawned;}}
    public List<PositionComponent> Positions { get { return positions; } }
    public RowComponent[] Rows { get { return mile.Rows; } }
    
    [Header("Chunk")]
    [SerializeField] Mile mile;
    [SerializeField] Transform entityParent;
    [SerializeField] GameObject active;

    [Header("Debug")]
    [SerializeField] bool completed;
    [SerializeField] bool spawned;
    [SerializeField] int roads;
    [SerializeField] int mileNumber;
    [SerializeField] List<PositionComponent> positions;

    bool chunkLoaded = false;

    //    completed: "bool",
    //     mileNumber: "uint32",
    //     //dynamic list of people who have helped build the mile
    //     entities: "bytes32[]",
    //       //dynamic list of people who have helped build the mile
    //     contributors: "bytes32[]",

    protected override void Awake() {
        base.Awake();
        if(Chunks == null) {StaticInit();}
    }

    protected override void InitDestroy() {
        base.InitDestroy();
        if(Chunks != null) {StaticDestroy();}
    }

    //add all position components to their proper chunks
    static void StaticInit() {

        Chunks = new Dictionary<int, ChunkComponent>();
        ChunkList = new List<ChunkComponent>();
        PositionsOnMiles = new Dictionary<PositionComponent, int>();

        TableManager pos = MUDWorld.FindTable<PositionComponent>();
        pos.OnComponentUpdated += PositionsToChunk;
        
    }

    static void StaticDestroy() {
        
        Chunks = null;
        ChunkList = null;
        PositionsOnMiles = null;

        TableManager pos = MUDWorld.FindTable<PositionComponent>();
        if(pos) pos.OnComponentUpdated -= PositionsToChunk;
    }

    static void PositionsToChunk(MUDComponent newPos) {
        PositionComponent pos = (PositionComponent)newPos;
        MUDEntity entity = newPos.Entity;

        if(PositionsOnMiles.ContainsKey(pos)) {return;}

        Debug.Log("CHUNK: " + newPos.Entity.gameObject.name, newPos.Entity);
        PositionsOnMiles[pos] = -1;
        entity.OnInitInfo += AddOnLoaded;
        pos.OnUpdatedInfo += AddOnUpdated;
    }

    static void AddOnLoaded(MUDEntity entity) {
        AddEntityToChunk(entity.GetMUDComponent<PositionComponent>());
    }

    static void AddOnUpdated(MUDComponent c, UpdateInfo update) {
        if(c.Entity.HasInit == false) return;
        AddEntityToChunk((PositionComponent)c);
    }

    static void AddEntityToChunk(PositionComponent pos) {

        int mile = (int)PositionComponent.PositionToMile(pos.Pos);
        ChunkComponent chunk = GetChunk(mile);

        if(chunk == null) return; //this can happen to certain objects like the Carriage that don't have a position
        chunk.PositionOnMile(pos);
    }

    void PositionOnMile(PositionComponent newPos) {

        int onMile = PositionsOnMiles[newPos];

        //remove from old mile or we're still on the same mile
        if(onMile != -1) {
            int newMile = (int)PositionComponent.PositionToMile(newPos.Pos);
            if(onMile == newMile) return;
            GetChunk(onMile).Positions.Remove(newPos);
        }

        //add to mile dictionary and list
        PositionsOnMiles[newPos] = mileNumber;
        Positions.Add(newPos);

        //parent
        newPos.Entity.transform.parent = Objects;
       
    }

    
    void LoadChunk() {

        //IMPORTANT
        //we must have loaded MapConfigComponent and RoadConfigComponent before we set up chunks
        //IMPORTANT 
        if (MapConfigComponent.Instance == null || RoadConfigComponent.Instance == null) { Debug.LogError("Can't setup chunk"); return;}
        if(Chunks.ContainsKey(mileNumber)) { Debug.LogError("Chunk " + mileNumber + " already exists", this); return;}

        Entity.SetName("MILE - " + mileNumber);
        Entity.transform.parent = WorldScroll.Instance.transform;
        positions = new List<PositionComponent>();

        gameObject.name = "CHUNK - " + mileNumber;
        transform.position = Vector3.forward * mileNumber * MapConfigComponent.Height;

        mile.Init(this);
        active.SetActive(false);

        ChunkList.Add(this);
        Chunks.Add(mileNumber, this);


        if (completed) {
        } else {
        }

        chunkLoaded = true;

        if (mileNumber == GameStateComponent.MILE_COUNT) { 
            ActiveChunk = this; 
            OnActiveChunk?.Invoke();
        }

        Debug.Log("CHUNK " + mileNumber + " Completed: " + completed.ToString() + " Spawned: " + spawned.ToString() + " TOTAL: " + ChunkList.Count, this);


    }

    protected override IMudTable GetTable() {return new ChunkTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        // Debug.Log("Chunk: " + eventType.ToString());
        ChunkTable table = (ChunkTable)update;

        completed = table.completed != null ? (bool)table.completed : completed;
        spawned = table.spawned != null ? (bool)table.spawned : spawned;
        mileNumber = table.mile != null ? (int)table.mile : mileNumber;
        roads = table.roads != null ? (int)table.roads : roads;

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        active.SetActive(!completed);

        if (chunkLoaded == false) {
            LoadChunk();
        }

        if(ActiveChunk == this) {
            OnChunkUpdate?.Invoke();
        }

    }




}
