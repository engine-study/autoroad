using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using DefaultNamespace;

public enum RoadState { None, Shoveled, Filled, Paved }

public class RoadComponent : MUDComponent
{
    [Header("Road")]
    public RoadState state;

    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType)
    {
        base.UpdateComponent(update, eventType);


        RoadTable table = (RoadTable)update;

        state = table.value != null? (RoadState)table.value : state;
        // entity.gameObject.transform.position = new Vector3(position.x, 0f, position.y);


    }
}
