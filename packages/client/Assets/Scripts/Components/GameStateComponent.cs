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
    public static System.Action<int> OnMileCompleted;
    public static GameStateComponent Instance;
    public static float MILE_DISTANCE {get { return MapConfigComponent.Height; } }
    public static float MILE_COUNT = -1;
    float lastMile = -1;

    [Header("Debug")]
    [SerializeField] protected int mile;


    protected override IMudTable GetTable() {return new GameStateTable();}
    protected override void UpdateComponent(IMudTable update, UpdateInfo newInfo) {
        GameStateTable table = (GameStateTable)update;

        mile = table.miles != null ? (int)table.miles : mile;
        MILE_COUNT = mile;

        Debug.Log("[GAMECOMPONENT] Mile: " + mile);

        Entity.SetName("WORLD");
        Entity.transform.parent = null;


        OnGameStateUpdated?.Invoke();
        if(Loaded && MILE_COUNT != lastMile && lastMile != -1) {
            OnMileCompleted?.Invoke((int)lastMile);
        }

        lastMile = MILE_COUNT;

    }

    protected override void Awake() {
        base.Awake();
        Instance = this;
    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;

    }
}
