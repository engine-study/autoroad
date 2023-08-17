using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using Cysharp.Threading.Tasks;

public class ChunkComponent : MUDComponent {


    [Header("State")]
    [SerializeField] protected bool completed;
    [SerializeField] protected int pieces;
    [SerializeField] protected int mileNumber;
    [SerializeField] protected int mileStartHeight;
    [SerializeField] protected RowComponent[] rows;

    [Header("Reference")]
    [SerializeField] protected Transform groundParent;
    [SerializeField] protected Transform groundLeft, groundRight;
    [SerializeField] protected Transform spawnLeft, spawnRight;
    [SerializeField] protected RowComponent rowPrefab;

    int rowTotal;
    int widthSize;

    public GameObject activeObjects;
    bool createdChunk = false;

    //    completed: "bool",
    //     mileNumber: "uint32",
    //     //dynamic list of people who have helped build the mile
    //     entities: "bytes32[]",
    //       //dynamic list of people who have helped build the mile
    //     contributors: "bytes32[]",

    protected override void Awake() {
        base.Awake();

        activeObjects.SetActive(false);
                
        Init();

    }


    void Init() {

        rowTotal = RoadConfigComponent.Height;
        rows = new RowComponent[rowTotal];
        
        for (int i = 0; i < rowTotal; i++)
        {
            RowComponent newRow = Instantiate(rowPrefab, transform.position + Vector3.forward * i, Quaternion.identity, transform);
            newRow.chunk = this;
            newRow.name = "Row " + i;
            newRow.SpawnRoad(RoadConfigComponent.Width);

            rows[i] = newRow;
        }

        groundParent.localScale = Vector3.one + Vector3.forward * (rowTotal-1);

        groundLeft.localPosition = Vector3.right * (RoadConfigComponent.Left - .5f);
        groundRight.localPosition = Vector3.right * (RoadConfigComponent.Right + .5f);

        groundLeft.localScale = Vector3.one + Vector3.right * (MapConfigComponent.PlayWidth - RoadConfigComponent.Right - 1f);
        groundRight.localScale = Vector3.one + Vector3.right * (MapConfigComponent.PlayWidth - RoadConfigComponent.Right - 1f);

        spawnLeft.localPosition = Vector3.right * (-MapConfigComponent.PlayWidth - .5f);
        spawnRight.localPosition = Vector3.right * (MapConfigComponent.PlayWidth + .5f);

    }

    protected override IMudTable GetTable() {return new ChunkTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        // Debug.Log("Chunk: " + eventType.ToString());

        ChunkTable table = (ChunkTable)update;

        completed = table.completed != null ? (bool)table.completed : completed;
        mileNumber = table.mile != null ? (int)table.mile : mileNumber;
        pieces = table.pieces != null ? (int)table.pieces : pieces;

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        activeObjects.SetActive(!completed);

        if (createdChunk == false) {
            CreateChunk();
        }

    }

    public async UniTaskVoid CreateChunk() {

        Entity.SetName("MILE - " + mileNumber);
        Entity.transform.parent = WorldScroll.Instance.transform;

        gameObject.name = "CHUNK - " + mileNumber;

        //we need the roadconfig info and the road pieces to have spawned to load the chunk
        //lots of waiting
        while (RoadConfigComponent.Width == 0) {
            await UniTask.Delay(500);
        }

        while (BoundsComponent.Left == 0) {
            await UniTask.Delay(500);
        }

        mileStartHeight = mileNumber * RoadConfigComponent.Height;
        transform.position = Vector3.forward * mileStartHeight;

        if (completed) {

            //create static combined mesh of road
            for (int i = 0; i < rows.Length; i++) {

            }

        } else {

            // //search the table for entities pertaining to the road
            // for (int y = mileStartHeight; y < mileStartHeight + RoadConfigComponent.Height; y++) {
            //     for (int x = 0; x < RoadConfigComponent.Width; x++) {

            //         //try to find the road component and add it to the row if one exists
            //         string entity = MUDHelper.Keccak256("Road", x + RoadConfigComponent.Left, y);
            //         RoadComponent c = TableManager.FindComponent<RoadComponent>(entity);

            //         int yIndex = y - RoadConfigComponent.Height;
            //         AddRoadComponent(entity, c, x, y);

            //     }
            // }
        }

        createdChunk = true; 
    }

    public void AddRoadComponent(string entity, RoadComponent c, int x, int y) {
        Debug.Log("Adding " + x + " " + y + " Rows: " + rows.Length + " Mile: " + c.Mile + " " + mileNumber, c);
        rows[y].SetRoadBlock(entity, x + RoadConfigComponent.Right, c);
    }

}
