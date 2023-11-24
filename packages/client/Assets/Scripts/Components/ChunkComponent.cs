using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mudworld;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ChunkComponent : MUDComponent {
    
    public static System.Action OnChunkUpdate;
    public static System.Action OnMileSpawned;
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
    [SerializeField] GameObject incomplete;

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

    public void Toggle(bool toggle) {
        mile.ToggleVisible(toggle);
        entityParent.gameObject.SetActive(toggle);
    }

    public void ToggleCurrentMile(bool toggle) {
        mile.ToggleCurrentMile(toggle);
    }

    void RegisterChunk() {

        //IMPORTANT
        //we must have loaded MapConfigComponent and RoadConfigComponent before we set up chunks
        //IMPORTANT 
        if (MapConfigComponent.Instance == null || RoadConfigComponent.Instance == null) { Debug.LogError("Can't setup chunk"); return;}

        Entity.SetName("MILE - " + mileNumber);

        // Entity.transform.parent = WorldScroll.Instance.transform;
        transform.parent = WorldScroll.Instance.transform;

        positions = new List<PositionComponent>();

        gameObject.name = "CHUNK - " + mileNumber;
        transform.position = Vector3.forward * mileNumber * MapConfigComponent.Height;

        ToggleCurrentMile(false);

        mile.Init(this);

        ChunkLoader.RegisterChunk(this);

        if (completed) {
        } else {
        }

        chunkLoaded = true;

    }

    protected override MUDTable GetTable() {return new ChunkTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        // Debug.Log("Chunk: " + eventType.ToString());
        ChunkTable table = (ChunkTable)update;

        bool wasSpawned = spawned;

        completed = table.Completed != null ? (bool)table.Completed : completed;
        spawned = table.Spawned != null ? (bool)table.Spawned : spawned;
        mileNumber = table.Mile != null ? (int)table.Mile : mileNumber;
        roads = table.Roads != null ? (int)table.Roads : roads;

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        incomplete.SetActive(!completed);

        if (chunkLoaded == false) {
            RegisterChunk();
        }

        if(ChunkLoader.ActiveChunk == this) {
            OnChunkUpdate?.Invoke();
        }

        if(Loaded && mileNumber == GameStateComponent.MILE_COUNT && wasSpawned != spawned) {
            OnMileSpawned?.Invoke();
        }

    }




}
