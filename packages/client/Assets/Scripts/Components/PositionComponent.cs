using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;

public class PositionComponent : MUDComponent {
    public Vector3 Pos { get { return position3D; } }

    [Header("Position")]
    [SerializeField] protected Vector2 position2D;
    [SerializeField] protected Vector3 position3D;
    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType) {
        base.UpdateComponent(update, eventType);

        PositionTable table = (PositionTable)update;

        // Debug.Log(eventType.ToString() + ": " + gameObject.name);
        // Debug.Log("X: " + (table.x != null ? table.x.ToString() : "no"));
        // Debug.Log("Y: " + (table.y != null ? table.y.ToString() : "no"));

        if (table.x == null) { Debug.LogError("No x", this); }
        if (table.y == null) { Debug.LogError("No y", this); }

        position2D = new Vector2(table.x ?? position2D.x, table.y ?? position2D.y);
        transform.position = new Vector3(position2D.x, 0f, position2D.y);

        position3D = transform.position;

        // entity.gameObject.transform.position = new Vector3(position.x, 0f, position.y);


    }

}
