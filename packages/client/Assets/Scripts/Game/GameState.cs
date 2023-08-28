using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;
using mud.Unity;
using DefaultNamespace;
using IWorld.ContractDefinition;
using System;
using Cysharp.Threading.Tasks;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GamePhase { Lobby, Game, PostGame, _Count }
public class GameState : MonoBehaviour {
    public static GameState Instance;

    [Header("Test")]
    [SerializeField] bool freshStart = false; 

    [Header("Game")]
    [SerializeField] GamePhase phase;
    [SerializeField] RecieverMUD reciever;
    [SerializeField] TableManager playerTable;
    [SerializeField] TableManager [] tables;

    [Header("UI")]
    [SerializeField] GameObject editorObjects;

    [Header("Debug")]
    [SerializeField] PlayerMUD localPlayer;

    void Awake() {
        Instance = this;
        editorObjects.SetActive(false);

        NetworkManager.OnInitialized += SetupGame;
        NetworkManager.OnInitialized += LoadServer;
        SPEvents.OnLocalPlayerSpawn += RecieverPlayer;
        GameStateComponent.OnGameStateUpdated += GameStateUpdated;
    }

    void Start() {

    }

    void OnDestroy() {

        NetworkManager.OnInitialized -= SetupGame;
        NetworkManager.OnInitialized -= LoadServer;
        SPEvents.OnLocalPlayerSpawn -= RecieverPlayer;
        GameStateComponent.OnGameStateUpdated -= GameStateUpdated;

        Instance = null;
    }

    async void SetupGame() {
        await GameSetup();
    }


    void LoadServer() {
        LoadMap();
    }

    async UniTask LoadMap() {

        while(TableSpawner.Loaded == false) {await UniTask.Delay(500);}
        while(BoundsComponent.Instance == null && MapConfigComponent.Instance == null && GameStateComponent.Instance == null) {await UniTask.Delay(500);}

        for (int i = 0; i < tables.Length; i++) { tables[i].gameObject.SetActive(true); }

        SPEvents.OnServerLoaded?.Invoke();

    }

    async UniTask GameSetup() {

        while(TableSpawner.Loaded == false) {await UniTask.Delay(500);}
        while(BoundsComponent.Instance == null && MapConfigComponent.Instance == null && GameStateComponent.Instance == null) {await UniTask.Delay(500);}
        
        //destroy the player if we want to simulate the login sequence
        if (freshStart) {
            await TxManager.Send<DestroyPlayerAdminFunction>();
            //while(player is not null) {}
        }

        //wait for name table
        while(MUDWorld.FindTable<NameComponent>()?.Loaded == false) {await UniTask.Delay(500);}
        NameTable localName = MUDWorld.FindValue<NameTable>(NetworkManager.LocalAddress);

        //create our player name
        if (localName == null) {

            if (SPGlobal.IsDebug) {

                Debug.Log("Making Name");
                while(await MotherUI.Mother.playerCreate.MakeName() == false) {await UniTask.Delay(2000);}

            } else {
                Debug.Log("Choosing Name");
                MotherUI.TogglePlayerCreation(true);
            }

            //wait for the name
            while(string.IsNullOrEmpty(NameComponent.LocalName)) {await UniTask.Delay(500);}

        }
        
        //wait for name component to spawn
        while(NameComponent.LocalName == null) {await UniTask.Delay(500);}

        //wait for player table
        while(MUDWorld.FindTable<PlayerComponent>().Loaded == false) {await UniTask.Delay(500);}
        while(MUDWorld.FindTable<HealthComponent>().Loaded == false) {await UniTask.Delay(500);}
        PlayerTable playerTable = MUDWorld.FindValue<PlayerTable>(NetworkManager.LocalAddress);
        HealthComponent healthComponent = MUDWorld.FindComponent<HealthComponent>(NetworkManager.LocalAddress);

        if (playerTable == null || healthComponent == null || healthComponent.Health < 1) {

            if(SPGlobal.IsDebug) {
                int x = BoundsComponent.Right + 1;
                int y = BoundsComponent.Up;
                Debug.Log("Spawning player at " + x + "," + y);
                while(await TxManager.Send<SpawnFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y)) == false)  {y--; await UniTask.Delay(2000);}
            } else {
                Debug.Log("Choosing spawn");
                MotherUI.TogglePlayerSpawning(true);
            }

        }

        //wait for the player
        while(PlayerMUD.LocalPlayer == null) {await UniTask.Delay(500);}

        //spawn debug road elements
        while(GameStateComponent.PlayerCount == 0) {await UniTask.Delay(500);}
        if(GameStateComponent.PlayerCount == 1) {
            // Debug.Log("Spawning debug road", this);
            // TxManager.Send<DebugMileFunction>(System.Convert.ToInt32(0));
        }
    }


    //player is spawned
    void RecieverPlayer() {
        SPPlayer.LocalPlayer.SetReciever(reciever);
        localPlayer = SPPlayer.LocalPlayer as PlayerMUD;
    }

    void GameStateUpdated() {

    }

    void MileCompletion() {

    }

    void Update() {
        UpdateInput();
    }

    void UpdateInput() {

        if(Input.GetKeyDown(KeyCode.F)) {

            if(Screen.fullScreen) {
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            } else {
                Screen.SetResolution(Screen.resolutions[0].width, Screen.resolutions[0].height, FullScreenMode.FullScreenWindow);
            }
        }

    }


// #if UNITY_EDITOR
//     [MenuItem("Engine/Game State &q")]
//     static void TogglePhase() {
//         GameState gs = FindObjectOfType<GameState>();

//         if (!gs) { return; }

//         gs.TogglePhase((GamePhase)((int)(gs.phase + 1) % (int)GamePhase._Count));
//         Selection.activeGameObject = gs.phaseUI[(int)gs.phase].gameObject;

//     }
// #endif


}
