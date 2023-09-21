using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using Cysharp.Threading.Tasks;

public class ChunkComponent : MUDComponent
{

    public static System.Action OnActiveChunk;
    public static System.Action OnChunkUpdate;
    
    public static ChunkComponent ActiveChunk;
    public static ChunkComponent GetChunk(int mile) {Chunks.TryGetValue(mile, out var chunk); return chunk; }
    public static Dictionary<int, ChunkComponent> Chunks;

    public Mile Mile {get{return mile;}}
    public RowComponent[] Rows { get { return mile.Rows; } }
    public bool Completed {get{return completed;}}
    public bool Spawned {get{return spawned;}}
    
    [Header("Chunk")]
    [SerializeField] Mile mile;
    [SerializeField] GameObject activeObjects;

    [Header("Debug")]
    [SerializeField] bool completed;
    [SerializeField] bool spawned;
    [SerializeField] int roads;
    [SerializeField] int mileNumber;
    [SerializeField] int mileStartHeight;

    bool chunkLoaded = false;

    //    completed: "bool",
    //     mileNumber: "uint32",
    //     //dynamic list of people who have helped build the mile
    //     entities: "bytes32[]",
    //       //dynamic list of people who have helped build the mile
    //     contributors: "bytes32[]",

    protected override void Awake() {
        base.Awake();

        if(Chunks == null) { Chunks = new Dictionary<int, ChunkComponent>();}
        
        Init();

    }

    void Init() {

        //IMPORTANT
        //we must have loaded MapConfigComponent and RoadConfigComponent before we set up chunks
        //IMPORTANT 

        activeObjects.SetActive(false);
        mile.SetupMile(this);

    }

    protected override IMudTable GetTable() {return new ChunkTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        // Debug.Log("Chunk: " + eventType.ToString());
        ChunkTable table = (ChunkTable)update;

        completed = table.completed != null ? (bool)table.completed : completed;
        spawned = table.spawned != null ? (bool)table.spawned : spawned;
        mileNumber = table.mile != null ? (int)table.mile : mileNumber;
        roads = table.roads != null ? (int)table.roads : roads;

        Debug.Log("CHUNK " + mileNumber + " Completed: " + completed.ToString() + " Spawned: " + spawned.ToString(), this);

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        activeObjects.SetActive(!completed);

        if (chunkLoaded == false) {
            CreateChunk();
        }

        if(ActiveChunk == this) {
            OnChunkUpdate?.Invoke();
        }

    }

    public async UniTaskVoid CreateChunk() {

        if (MapConfigComponent.Instance == null || RoadConfigComponent.Instance == null) { Debug.LogError("Can't setup chunk"); return; }

        if(Chunks.ContainsKey(mileNumber)) {
            Debug.LogError("Chunk " + mileNumber + " already exists", this);
            return;
        }

        Entity.SetName("MILE - " + mileNumber);
        Entity.transform.parent = WorldScroll.Instance.transform;

        gameObject.name = "CHUNK - " + mileNumber;

        mileStartHeight = mileNumber * MapConfigComponent.Height;
        transform.position = Vector3.forward * mileStartHeight;

        if (completed) {
        } else {
        }


        chunkLoaded = true;

        Chunks.Add(mileNumber, this);
        if (mileNumber == GameStateComponent.MILE_COUNT) { 
            ActiveChunk = this; 
            OnActiveChunk?.Invoke();
        }
    }



}
