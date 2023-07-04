using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;

public class PositionComponent : MUDComponent
{
    public Vector3 Pos { get { return position3D; } }

    [Header("Position")]
    [SerializeField] protected Vector2 position2D;
    [SerializeField] protected Vector3 position3D;
    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType)
    {
        base.UpdateComponent(update, eventType);

        PositionTable pos = (PositionTable)update;

        position2D = new Vector2((float)pos.x, (float)pos.y);
        transform.position = new Vector3(position2D.x, 0f, position2D.y);
        position3D = transform.position;
        
        // entity.gameObject.transform.position = new Vector3(position.x, 0f, position.y);


    }

}
