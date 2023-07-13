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
    public static GameStateComponent Instance;
    public const float MILE_DISTANCE = 20f;
    public static float MILE_COUNT;
    public static float MILE_ENDPOS;

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

        MILE_COUNT = miles;
        MILE_ENDPOS = MILE_COUNT * MILE_DISTANCE;

        edge.transform.position = Vector3.forward * miles * MILE_DISTANCE;
        
    }

}
