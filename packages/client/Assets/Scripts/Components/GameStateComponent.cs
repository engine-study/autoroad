using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mudworld;
using mud;
using Cysharp.Threading.Tasks;
using UniRx;

public class GameStateComponent : MUDComponent {

    public static System.Action OnGameStateUpdated;
    public static System.Action<int> OnMileSummoned;
    public static System.Action<int> OnMileCompleted;
    public static GameStateComponent Instance;
    public static float MILE_DISTANCE {get { return MapConfigComponent.Height; } }
    public static float MILE_COUNT = -1;
    float lastMile = -1;

    [Header("Debug")]
    [SerializeField] protected int mile;


    protected override MUDTable GetTable() {return new GameStateTable();}
    protected override void UpdateComponent(MUDTable update, UpdateInfo newInfo) {

        if(Instance == null) {
            Instance = this;
            Entity.SetName("WORLD");
            Entity.transform.parent = null;
        }

        GameStateTable table = (GameStateTable)update;

        lastMile = MILE_COUNT;

        mile = table.Miles != null ? (int)table.Miles : mile;
        MILE_COUNT = mile;

        Debug.Log("[GAMECOMPONENT] Mile: " + mile);

        OnGameStateUpdated?.Invoke();

        if(PlayerComponent.LocalPlayer && MILE_COUNT != lastMile && lastMile != -1) {
            OnMileCompleted?.Invoke((int)lastMile);
        }

    }

    protected override void OnDestroy() {
        base.OnDestroy();
        Instance = null;

    }
}
