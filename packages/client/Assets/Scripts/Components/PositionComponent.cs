using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class PositionComponent : MUDComponent {
    public Vector3 Pos { get { return position3D; } }
    public Vector3 PosLayer { get { return position3DLayer; } }


    [Header("Position")]
    [SerializeField] private int layer = 0;
    [SerializeField] private Vector2 position2D;
    [SerializeField] private Vector3 position3D;
    [SerializeField] private Vector3 position3DLayer;


    public void SetLayer(int newLayer) {
        layer = newLayer;
        position3DLayer = new Vector3(position2D.x, layer, position2D.y);
    }

    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        PositionTable table = update as PositionTable;

        position2D = new Vector2((int)table.x, (int)table.y);
        position3D = new Vector3(position2D.x, 0f, position2D.y);
        position3DLayer = new Vector3(position2D.x, layer, position2D.y);

        transform.position = position3D;

    }

}
