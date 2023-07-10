using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum RockType { Raw, Statumen, Rudus, Nucleus, Pavimentum, _Count }
public class RockComponent : MUDComponent
{
    public int Stage { get { return stage; } }

    [Header("Rock")]
    [SerializeField] protected int stage = -1;
    [SerializeField] protected RockType rockType;
    [SerializeField] SPAudioSource source;

    [SerializeField] GameObject[] stages;
    [SerializeField] AudioClip [] sfx_break;
    RockType lastStage = RockType._Count;
    protected override void Awake() {
        base.Awake();
        rockType = RockType._Count;
        stage = -1;
    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType)
    {

        base.UpdateComponent(update, eventType);

        RockTable rockUpdate = (RockTable)update;

        if (rockUpdate == null)
        {
            Debug.LogError("No rockUpdate", this);
        }
        else
        {

            // stage = rockUpdate.rockType != null ? (int)rockUpdate.rockType : stage;
            // rockType = rockUpdate.rockType != null ? (RockType)rockUpdate.rockType : rockType;
            Debug.Log(rockUpdate.value.ToString());
            
            rockType = rockUpdate.value != null ? (RockType)rockUpdate.value : RockType._Count;

        }

        if(lastStage != rockType) {

            source.PlaySound(sfx_break);
            
            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].SetActive(i == (int)rockType);
            
            }
        
        }

        lastStage = rockType;

    }

}
