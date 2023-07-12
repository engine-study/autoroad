using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;

public class GameStateComponent : MUDComponent
{
    [Header("Position")]
    [SerializeField] protected int miles;
    [SerializeField] protected WorldScroll scroll;
    [SerializeField] protected GameObject edge; 
    protected override void UpdateComponent(IMudTable update, UpdateEvent eventType)
    {
        base.UpdateComponent(update, eventType);

        GameStateTable table = (GameStateTable)update;

        miles = (int)table.miles;
        scroll.SetMaxMile(miles);

        edge.transform.position = Vector3.forward * miles * WorldScroll.MILE_DISTANCE;
        
    }

}
