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
    public static Action GameStarted;
    public static GameState Instance;
    public static bool GameReady {get{return Instance.gameReady;}}
    public static bool GamePlaying {get{return Instance.gamePlaying;}}

    [Header("Test")]
    [SerializeField] bool freshStart = false; 
    [SerializeField] bool autoJoin = false; 

    [Header("Game")]
    [SerializeField] TableSpawner mainTables;
    [SerializeField] TableManager chunkTable;
    [SerializeField] TableManager [] tables;
    [SerializeField] RecieverMUD reciever;

    [Header("UI")]
    [SerializeField] GameObject editorObjects;

    [Header("Debug")]
    [SerializeField] PlayerMUD localPlayer;
    [SerializeField] bool gameReady = false; 
    [SerializeField] bool gamePlaying = false; 

    void Awake() {
        Instance = this;
        editorObjects.SetActive(false);
        
        SPEvents.OnLocalPlayerSpawn += RecieverPlayer;
        GameStateComponent.OnGameStateUpdated += GameStateUpdated;
    }
    
    void OnDestroy() {

        SPEvents.OnLocalPlayerSpawn -= RecieverPlayer;
        GameStateComponent.OnGameStateUpdated -= GameStateUpdated;

        Instance = null;
    }

    void JoinGaul() {
        NetworkManager.Instance.Connect();
    }

    void LeaveGaul() {
        gameReady = false;
    }

    void Start() {

        if(autoJoin) {
            JoinGaul();
        }

        GameSetup();
        LoadMap();

    }

    async UniTask LoadMap() {

        //Load network
        while(NetworkManager.Initialized == false) {await UniTask.Delay(100);}

        //Load world states
        Debug.Log("---GAMESTATE--- LOAD WORLD");
        while(mainTables.Loaded == false) {await UniTask.Delay(100);}
        while(BoundsComponent.Instance == null && MapConfigComponent.Instance == null && GameStateComponent.Instance == null) {await UniTask.Delay(100);}

        //Load all chunks
        Debug.Log("---GAMESTATE--- LOAD CHUNKS");
        chunkTable.SubscribeAll();
        while(ChunkComponent.ChunkList.Count < GameStateComponent.MILE_COUNT+1) {await UniTask.Delay(100);}

        //Load all entities with position component
        Debug.Log("---GAMESTATE--- LOAD ALL");
        for (int i = 0; i < tables.Length; i++) { tables[i].SubscribeAll(); }

        await UniTask.Delay(1000);

        SPEvents.OnServerLoaded?.Invoke();
        Debug.Log("---GAMESTATE--- DONE");

    }

    public static void PlayGame() {

        Instance.gamePlaying = true;
        SPEvents.OnPlayGame?.Invoke();
                
        Instance.PlayGameLoop();

    }
    async UniTask PlayGameLoop() {

        while(NetworkManager.Initialized == false) {await UniTask.Delay(100);}

        //wait until the map is setup
        while (ChunkComponent.ActiveChunk == null || (ChunkComponent.ActiveChunk.MileNumber == 0 && ChunkComponent.ActiveChunk.Spawned == false)) { await UniTask.Delay(200); }

        //wait for name table
        while(MUDWorld.FindTable<NameComponent>()?.Loaded == false) {await UniTask.Delay(500);}
        NameTable localName = MUDWorld.FindValue<NameTable>(NetworkManager.LocalAddress);


        //-----------------------------------------------------------------------
        //NAMING
        //-----------------------------------------------------------------------
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

        //-----------------------------------------------------------------------
        //SPAWNING
        //-----------------------------------------------------------------------
        if (playerTable == null || healthComponent == null || healthComponent.Health < 1) {

            if(SPGlobal.IsDebug) {
                int x = BoundsComponent.Right + 1;
                int y = BoundsComponent.Up;
                Debug.Log("Spawning player at " + x + "," + y);
                if (await TxManager.SendUntilPasses<SpawnFunction>( PositionComponent.PositionToTransaction(new Vector3(x,0,y)) ) == false) { Debug.LogError("Couldn't spawn"); }

            } else {
                Debug.Log("Choosing spawn");
                MotherUI.TogglePlayerSpawning(true);
            }

        }

        //wait for the player
        while(PlayerMUD.LocalPlayer == null) {await UniTask.Delay(100);}

        //spawn debug road elements
        while(PlayerComponent.PlayerCount == 0) {await UniTask.Delay(500);}
        if(PlayerComponent.PlayerCount == 1) {
            // Debug.Log("Spawning debug road", this);
            // TxManager.Send<DebugMileFunction>(System.Convert.ToInt32(0));
        }

        MotherUI.Mother.StartPlaying();

    }


    async UniTask GameSetup() {
        
        //destroy the player if we want to simulate the login sequence
        if (freshStart) {
            await TxManager.SendQueue<DestroyPlayerAdminFunction>();
            //while(player is not null) {}
        }

        Debug.Log("Game Ready", this);
        gameReady = true;
        SPEvents.OnGameReady?.Invoke();

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
