using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

public class Position : MUDComponent
{
   [Header("Position")]
    public Vector2 position;

    public override void UpdateComponent(mud.Client.IMudTable update, TableEvent eventType)
    {
        base.UpdateComponent(update, eventType);

        PositionTable pos = (PositionTable)update;
        position = new Vector2((float)pos.x, (float)pos.y);
        transform.position = new Vector3(position.x, 0f, position.y);
        entity.gameObject.transform.position = new Vector3(position.x, 0f, position.y);
    }

}
