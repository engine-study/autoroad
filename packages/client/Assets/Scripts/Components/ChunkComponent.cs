using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using Cysharp.Threading.Tasks;

public class ChunkComponent : MUDComponent {

    [Header("Position")]
    [SerializeField] protected bool completed;
    [SerializeField] protected int mileNumber;
    [SerializeField] protected int mileStartHeight;
    [SerializeField] protected RowComponent[] rows;
    public GameObject activeObjects;


    //    completed: "bool",
    //     mileNumber: "uint32",
    //     //dynamic list of people who have helped build the mile
    //     entities: "bytes32[]",
    //       //dynamic list of people who have helped build the mile
    //     contributors: "bytes32[]",

    protected override void Awake() {
        base.Awake();

        activeObjects.SetActive(false);
    }
    protected override IMudTable GetTable() {return new ChunkTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        // Debug.Log("Chunk: " + eventType.ToString());

        ChunkTable table = (ChunkTable)update;

        if (update == null) {
            Debug.LogError("Chunk is null", this);
        }
        if (table == null) {
            Debug.LogError("Table is null", this);
        }

        completed = table.completed != null ? (bool)table.completed : completed;
        mileNumber = table.mile != null ? (int)table.mile : mileNumber;

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        activeObjects.SetActive(!completed);

        if (newInfo.UpdateType == UpdateType.SetRecord) {
            CreateChunk();
        }

    }

    public async UniTaskVoid CreateChunk() {

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
    }

    public void AddRoadComponent(string entity, RoadComponent c, int x, int y) {
        rows[y].SetRoadBlock(entity, x + RoadConfigComponent.Right, c);
    }



}
