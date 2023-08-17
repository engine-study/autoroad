using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefaultNamespace;
using mud.Client;
using NetworkManager = mud.Unity.NetworkManager;
using Cysharp.Threading.Tasks;
using UniRx;

public class GameStateComponent : MUDComponent {

    public static System.Action OnGameStateUpdated;
    public static System.Action OnMileCompleted;
    public static GameStateComponent Instance;
    public static float MILE_DISTANCE {get { return RoadConfigComponent.Height; } }
    public static float MILE_COUNT;
    public static int PlayerCount;

    [Header("Debug")]
    [SerializeField] protected int currentMile;
    [SerializeField] protected int playerCount;


    protected override IMudTable GetTable() {return new GameStateTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {

        Instance = this;
        
        GameStateTable table = (GameStateTable)update;

        currentMile = table.miles != null ? (int)table.miles : currentMile;;

        PlayerCount = table.playerCount != null ? (int)table.playerCount : PlayerCount;
        playerCount = PlayerCount;

        MILE_COUNT = currentMile;

        // Debug.Log("Game State:");
        // Debug.Log("Miles: " + (table.miles != null ? (int)table.miles : "null"));
        // Debug.Log("Players: " + (table.playerCount != null ? (int)table.playerCount : "null"));

        Entity.SetName("WORLD");
        Entity.transform.parent = GameState.Instance.transform;

        OnGameStateUpdated?.Invoke();

        if(Loaded) {
            OnMileCompleted?.Invoke();
        }

    }

}
