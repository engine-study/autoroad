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
    [SerializeField] protected int stage = -1;
    [SerializeField] protected RockType rockType;
    [SerializeField] SPAudioSource source;

    [SerializeField] GameObject[] stages;
    [SerializeField] AudioClip [] sfx_break;

    protected override void Awake() {
        base.Awake();

        stage = -1;
    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType)
    {

        base.UpdateComponent(update, eventType);

        int lastStage = stage;

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

        if(lastStage != -1) {

            source.PlaySound(sfx_break);
            
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].SetActive(i == stage);
            
            }
        
        }

    }

}
