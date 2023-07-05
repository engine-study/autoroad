using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class ChunkComponent : MUDComponent
{

    [Header("Position")]
    [SerializeField] protected bool completed;
    [SerializeField] protected float mileNumber;

    //    completed: "bool",
    //     mileNumber: "uint32",
    //     //dynamic list of people who have helped build the mile
    //     entities: "bytes32[]",
    //       //dynamic list of people who have helped build the mile
    //     contributors: "bytes32[]",

    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType)
    {
        base.UpdateComponent(update, eventType);

        // Debug.Log("Chunk: " + eventType.ToString());

        ChunkTable table = (ChunkTable)update;

        if (update == null)
        {
            Debug.LogError("Chunk is null", this);
        }
        if (table == null)
        {
            Debug.LogError("Table is null", this);
        }



        completed = table.completed != null ? (bool)table.completed : completed;
        mileNumber = table.mileNumber != null ? (float)table.mileNumber : mileNumber;

        // Debug.Log("MileSafe " + table.mileNumber.GetValueOrDefault(), this);
        // Debug.Log("MileTest " + table.mileNumber, this);
        // Debug.Log("Mile " + mileNumber, this);

        transform.position = Vector3.forward * mileNumber * 20f;


    }

}
