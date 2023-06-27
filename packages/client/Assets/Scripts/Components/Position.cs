using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;

public class Position : MUDComponent
{
    public Vector3 Pos { get { return position3D; } }

    [Header("Position")]
    public Vector2 position2D;
    public Vector3 position3D;
    public override void UpdateComponent(mud.Client.IMudTable update, TableEvent eventType)
    {

        PositionTable pos = (PositionTable)update;

        position2D = new Vector2((float)pos.x, (float)pos.y);
        transform.position = new Vector3(position2D.x, 0f, position2D.y);
        position3D = transform.position;
        
        // entity.gameObject.transform.position = new Vector3(position.x, 0f, position.y);

        base.UpdateComponent(update, eventType);

    }

}
