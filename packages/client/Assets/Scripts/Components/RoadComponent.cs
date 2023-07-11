using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;
using IWorld.ContractDefinition;

public enum RoadState { None, Shoveled, Filled, Paved }

public class RoadComponent : MUDComponent
{
    [Header("Road")]
    public RoadState state;
    public GameObject [] stages;
    RoadState lastStage = RoadState.None;

    protected override void Awake() {
        base.Awake();
        Debug.Log("Road awake", this);
        state = RoadState.None;
    }

    protected override void UpdateComponent(mud.Client.IMudTable update, UpdateEvent eventType)
    {

        base.UpdateComponent(update, eventType);

        RoadTable roadUpdate = (RoadTable)update;

        if (roadUpdate == null)
        {
            Debug.LogError("No roadUpdate", this);
        }
        else
        {
            
            state = roadUpdate.value != null ? (RoadState)roadUpdate.value : RoadState.None;

        }

        if(lastStage != state) {

            if(eventType == UpdateEvent.Update || eventType == UpdateEvent.Optimistic) {
                // source.PlaySound((int)state < 3 ? sfx_bigBreaks : sfx_smallBreaks);
                // fx_break.Play();
            }

            for (int i = 0; i < stages.Length; i++)
            {
                stages[i].SetActive(i == (int)state);
            }
        
        }

        lastStage = state;

    }

}
