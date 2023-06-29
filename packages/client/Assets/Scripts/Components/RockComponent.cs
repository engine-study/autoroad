using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum RockType { Statumen, Rudus, Nucleus, Pavimentum, _Count }
public class RockComponent : MUDComponent
{
    public int Stage { get { return stage; } }

    [Header("Rock")]
    [SerializeField] protected int stage;
    [SerializeField] protected RockType rockType;

    [SerializeField] GameObject[] stages;

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType)
    {

        base.UpdateComponent(update, eventType);

        RockTable rockUpdate = (RockTable)update;

        if (rockUpdate.size == null)
        {
            // Debug.LogError("No currentValue");

        }
        else
        {

            stage = (int)rockUpdate.size;
            rockType = (RockType)rockUpdate.rockType;



        }

        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == stage);
        }
    }

}
