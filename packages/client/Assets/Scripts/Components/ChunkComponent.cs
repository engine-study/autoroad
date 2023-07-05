using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class ChunkComponent : MUDComponent
{

    [Header("Position")]
    [SerializeField] protected bool completed;
    [SerializeField] protected int mileNumber;

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



        completed = table.completed.GetValueOrDefault() ;
        mileNumber = (int)table.mileNumber.GetValueOrDefault();

        transform.position = Vector3.forward * mileNumber * 20f;


    }

}