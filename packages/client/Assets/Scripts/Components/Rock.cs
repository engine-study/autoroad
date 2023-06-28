using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public class Rock : MUDComponent
{
    public int Stage { get { return stage; } }

    [Header("Rock")]
    [SerializeField] protected int stage;

    [SerializeField] GameObject [] stages;

    protected override void UpdateComponent(mud.Client.IMudTable update, TableEvent eventType) {

        base.UpdateComponent(update, eventType);

        RockTable rockUpdate = (RockTable)update;

        stage = (int)rockUpdate.size;

        for(int i = 0; i < stages.Length; i++) {
            stages[i].SetActive(i == stage);
        }

    }

}
