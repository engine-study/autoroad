using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ChunkComponent : MUDComponent {
    
    public static System.Action OnChunkUpdate;
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
        mile.Toggle(toggle);
        entityParent.gameObject.SetActive(toggle);
    }

    public void Highlight(bool toggle) {
        mile.Highlight(toggle);
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

        Highlight(false);

        mile.Init(this);

        ChunkLoader.RegisterChunk(this);

        if (completed) {
        } else {
        }

        chunkLoaded = true;

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

        incomplete.SetActive(!completed);

        if (chunkLoaded == false) {
            RegisterChunk();
        }

        if(ChunkLoader.ActiveChunk == this) {
            OnChunkUpdate?.Invoke();
        }

    }




}
