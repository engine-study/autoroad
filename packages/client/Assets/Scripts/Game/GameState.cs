using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud;
using mud;
using mudworld;
using IWorld.ContractDefinition;
using System;
using Cysharp.Threading.Tasks;
using Nethereum.Model;
using UniRx;




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
    public bool newGame = false; 
    public bool skipMenu = false; 

    [Header("Game")]
    [SerializeField] TableSpawner mainTables;
    [SerializeField] TableSpawner visualTables;
    [SerializeField] TableSpawner mileTables;
    [SerializeField] TableManager chunkTable;
    [SerializeField] TableManager [] tables;
    [SerializeField] RecieverMUD reciever;

    [Header("UI")]
    [SerializeField] GameObject editorObjects;
    [SerializeField] GameObject [] qualityObjects;

    [Header("Debug")]
    [SerializeField] PlayerMUD localPlayer;
    [SerializeField] bool gameReady = false; 
    [SerializeField] bool gamePlaying = false; 

    void Awake() {

        Instance = this;
        editorObjects.SetActive(false);
        
        ToggleQuality(false);

        SPEvents.OnLocalPlayerSpawn += RecieverPlayer;
        GameStateComponent.OnGameStateUpdated += GameStateUpdated;

    }
    
    void OnDestroy() {

        SPEvents.OnLocalPlayerSpawn -= RecieverPlayer;
        GameStateComponent.OnGameStateUpdated -= GameStateUpdated;

        Instance = null;
    }

    void LeaveGaul() {
        gameReady = false;
    }

    private async void Start() {
        await GameSetup();
        await NetworkManager.Instance.CreateNetwork();
        await LoadMap();
    }

    async UniTask LoadMap() {

        while(NetworkManager.Initialized == false) {await UniTask.Delay(100);}

        //Load world states
        Debug.Log("---GAMESTATE--- LOAD WORLD");
        await mainTables.Spawn();
        while(mainTables.Loaded == false) {await UniTask.Delay(100);}
        while(BoundsComponent.Instance == null || MapConfigComponent.Instance == null || GameStateComponent.Instance == null || RoadConfigComponent.Instance == null) {await UniTask.Delay(100);}

        //Load all chunks
        Debug.Log("---GAMESTATE--- LOAD CHUNKS");
        chunkTable.Spawn();
        while(ChunkLoader.HasLoadedAllChunks == false) { await UniTask.Delay(100);}

        //Load all entities with position component
        Debug.Log("---GAMESTATE--- LOAD ALL");
        for (int i = 0; i < tables.Length; i++) { tables[i].Spawn(); }

        await UniTask.Delay(1000);

        SPEvents.OnServerLoaded?.Invoke();
        Debug.Log("---GAMESTATE--- DONE");

    }

    public static async void PlayGame() {

        Debug.Log("Start playing");

        Instance.gamePlaying = true;
        SPEvents.OnPlayGame?.Invoke();

        await Instance.PlayAwait();

    }

    async UniTask PlayAwait() {
        await PlayGameLoop();
    }

    async UniTask PlayGameLoop() {

        Debug.Log("--Initialize--");
        while(NetworkManager.Initialized == false) {await UniTask.Delay(100);}
        while(ChunkLoader.ActiveChunk == null) { await UniTask.Delay(200); }

        await SetAccount();

        await SetName();

        await SetTutorial();

        await SetFirstMile();
        
        await SetPlayer();

        MotherUI.Mother.OnLocalPlayerReady();

    }

    async UniTask SetAccount() {
    
        if(skipMenu) {
            //use the default that NetworkManager gives us
        } else {
            //wait for name table        
            Debug.Log("--Make Account--");
            MotherUI.ToggleAccountCreation(true);

            while(MotherUI.Mother.accountCreate.Account == null) {await UniTask.Delay(500);}
        }

    }

    async UniTask SetName() {
        //wait for name table        
        Debug.Log("--Make Name--");
        while(MUDWorld.FindTable<NameComponent>()?.Loaded == false) {await UniTask.Delay(500);}
        NameComponent localName = MUDWorld.FindComponent<NameComponent>(NetworkManager.LocalKey);

        //-----------------------------------------------------------------------
        //NAMING
        //-----------------------------------------------------------------------
        if (localName == null) {

            if (SPGlobal.IsDebug) {

                Debug.Log("Making Name");
                while(localName == null) {  
                    if(await MotherUI.Mother.playerCreate.MakeName() == false) {await UniTask.Delay(1000);}
                    localName = MUDWorld.FindComponent<NameComponent>(NetworkManager.LocalKey);
                }


            } else {
                Debug.Log("Choosing Name");
                MotherUI.ToggleNameSelection(true);
            }

            //wait for the name
            while(string.IsNullOrEmpty(NameComponent.LocalName)) {await UniTask.Delay(500);}

        } else {
            Debug.Log("Has Name");
        }
        
        //wait for name component to spawn
        while(NameComponent.LocalName == null) {await UniTask.Delay(500);}
    }

    async UniTask SetTutorial() {

        if(MotherUI.Mother.tutorial.hasCompleted || skipMenu) return;
        MotherUI.Mother.tutorial.ToggleWindowOpen();

        while(MotherUI.Mother.tutorial.Active) {await UniTask.Delay(100);}
    }

    async UniTask SetFirstMile() {
        //wait until the map is setup
        Debug.Log("--Spawn Map--");
        while (ChunkLoader.ActiveChunk.MileNumber == 0 && ChunkLoader.ActiveChunk.Spawned == false) { 
            if(await TxManager.SendDirect<HelpSummonFunction>() == false) {await UniTask.Delay(1000);}
        }

    }
            
    async UniTask SetPlayer() {
        //wait for player table
        while(MUDWorld.FindTable<PlayerComponent>().Loaded == false) {await UniTask.Delay(500);}
        while(MUDWorld.FindTable<HealthComponent>().Loaded == false) {await UniTask.Delay(500);}
        PlayerComponent player = MUDWorld.FindComponent<PlayerComponent>(NetworkManager.LocalKey);
        HealthComponent healthComponent = MUDWorld.FindComponent<HealthComponent>(NetworkManager.LocalKey);

        //-----------------------------------------------------------------------
        //SPAWNING
        //-----------------------------------------------------------------------
        if (player == null || healthComponent == null || healthComponent.Health < 1) {

            if(SPGlobal.IsDebug) {
                int x = BoundsComponent.Right + 1;
                int y = BoundsComponent.Up;
                Debug.Log("Spawning player at " + x + "," + y);
                while (await TxManager.SendDirect<SpawnFunction>( PositionComponent.PositionToTransaction(new Vector3(x,0,y)) ) == false) { y--; Debug.LogError("Couldn't spawn"); await UniTask.Delay(1000); }
                

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

    }


    async UniTask GameSetup() {
        
        //destroy the player if we want to simulate the login sequence
        if (newGame) {
            PlayerPrefs.DeleteAll();
            // await TxManager.SendQueue<DestroyPlayerAdminFunction>();
            //while(player is not null) {}
        }

        Debug.Log("---GAMESTATE--- Game Ready", this);

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

    public void ToggleQuality() {
        ToggleQuality(!qualityObjects[0].activeSelf);
    }

    public void ToggleQuality(bool toggle) {
        for(int i = 0; i < qualityObjects.Length; i++) {
            qualityObjects[i].SetActive(toggle);
        }
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
