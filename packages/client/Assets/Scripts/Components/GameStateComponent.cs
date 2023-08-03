using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;

public class GameStateComponent : MUDComponent {
    public static GameStateComponent Instance;
    public const float MILE_DISTANCE = 20f;
    public static float MILE_COUNT;
    public static int PlayerCount;

    [Header("Game State")]
    [SerializeField] protected WorldScroll scroll;
    [SerializeField] protected GameObject edge;

    [Header("Debug")]
    [SerializeField] protected int miles;
    [SerializeField] protected int playerCount;


    protected override IMudTable GetTable() {return new GameStateTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        GameStateTable table = (GameStateTable)update;

        miles = table.miles != null ? (int)table.miles : miles;;

        PlayerCount = table.playerCount != null ? (int)table.playerCount : PlayerCount;
        playerCount = PlayerCount;

        scroll.SetMaxMile(miles);
        MILE_COUNT = miles;

        edge.transform.position = Vector3.forward * (miles * MILE_DISTANCE + MILE_DISTANCE);

        // Debug.Log("Game State:");
        // Debug.Log("Miles: " + (table.miles != null ? (int)table.miles : "null"));
        // Debug.Log("Players: " + (table.playerCount != null ? (int)table.playerCount : "null"));

        Entity.SetName("WORLD");

    }

}
