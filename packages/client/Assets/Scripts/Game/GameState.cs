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
    public static GamePhase Phase { get { return Instance.phase; } }
    public static GameState Instance;
    public static Action<GamePhase> OnPhaseUpdate;

    [Header("Test")]
    public bool freshStart = false; 

    [Header("Game")]
    public GamePhase phase;
    public RecieverMUD reciever;
    public TableManager playerTable;
    public WorldScroll scroll;


    [Header("UI")]
    public GameObject editorObjects;
    public PhaseUI[] phaseUI;

    [Header("Debug")]
    public PlayerMUD localPlayer;

    void Awake() {
        Instance = this;
        editorObjects.SetActive(false);

        NetworkManager.OnInitialized += SetupGame;
        NetworkManager.OnInitialized += LoadServer;
        SPEvents.OnLocalPlayerSpawn += RecieverPlayer;

    }

    void Start() {

        for (int i = 0; i < phaseUI.Length; i++) {
            phaseUI[i].ToggleWindowClose();
        }

        TogglePhase(phase);

    }

    void OnDestroy() {

        NetworkManager.OnInitialized -= SetupGame;
        NetworkManager.OnInitialized -= LoadServer;
        SPEvents.OnLocalPlayerSpawn -= RecieverPlayer;

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

        while(TableManager.FindTable<BoundsComponent>().HasInit == false) {await UniTask.Delay(500);}
        while(TableManager.FindTable<PlayerComponent>().HasInit == false) {await UniTask.Delay(500);}

        SPEvents.OnServerLoaded?.Invoke();

    }
    async UniTask GameSetup() {

        while(TableSpawner.Loaded == false) {await UniTask.Delay(500);}


        if(freshStart) {
            await TxManager.Send<DestroyPlayerFunction>();
        }

        //wait for name table
        while(TableManager.FindTable<NameComponent>().HasInit == false) {await UniTask.Delay(500);}
        NameTable localName = TableManager.FindValue<NameTable>(NetworkManager.LocalAddress);

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
        while(TableManager.FindTable<PlayerComponent>().HasInit == false) {await UniTask.Delay(500);}
        PlayerTable localPlayerComponent = TableManager.FindValue<PlayerTable>(NetworkManager.LocalAddress);

        if (localPlayerComponent == null) {

            if(SPGlobal.IsDebug) {
                int x = BoundsComponent.Right + 1;
                int y = BoundsComponent.Up - 5;
                Debug.Log("Spawning player at " + x + "," + y);
                while(await TxManager.Send<SpawnFunction>(System.Convert.ToInt32(x), System.Convert.ToInt32(y)) == false)  {await UniTask.Delay(2000);}
            } else {
                Debug.Log("Choosing spawn");
                MotherUI.TogglePlayerSpawning(true);
            }

        }

        //wait for the player
        while(PlayerComponent.LocalPlayerKey == null) {await UniTask.Delay(500);}

    }


    //player is spawned
    void RecieverPlayer() {
        SPPlayer.LocalPlayer.SetReciever(reciever);
        localPlayer = SPPlayer.LocalPlayer as PlayerMUD;
    }


    void Update() {
        UpdateInput();
    }

    void UpdateInput() {
        if (SPUIBase.CanInput && Input.GetMouseButtonDown(1)) {
            if (CursorMUD.Base) {
                SPCamera.SetFollow(CursorMUD.Base.Root);
            } else {
                SPCamera.SetFollow(null);
                float x = Mathf.Clamp(SPInput.MousePlanePos.x, BoundsComponent.Left, BoundsComponent.Right);
                float z = Mathf.Clamp(SPInput.MousePlanePos.z, BoundsComponent.Down, BoundsComponent.Up);
                SPCamera.SetTarget(new Vector3(x, 0f, z));
            }

        }

        if (SPUIBase.CanInput && Input.GetKeyDown(KeyCode.Space)) {
            if (localPlayer) {
                SPCamera.SetFollow(localPlayer.Root);
            }
        }
    }

    void UpdatePhase() {
        phaseUI[(int)phase].UpdatePhase();
    }

    public void TogglePhase(GamePhase newPhase) {

        phaseUI[(int)phase].ToggleWindow(false);

        if (phase == GamePhase.Lobby) {

        } else if (phase == GamePhase.Game) {

        } else if (phase == GamePhase.PostGame) {

        }

        phase = newPhase;
        phaseUI[(int)phase].ToggleWindow(true);

        if (phase == GamePhase.Lobby) {

        } else if (phase == GamePhase.Game) {

        } else if (phase == GamePhase.PostGame) {

        }

    }

#if UNITY_EDITOR
    [MenuItem("Engine/Game State &q")]
    static void TogglePhase() {
        GameState gs = FindObjectOfType<GameState>();

        if (!gs) { return; }

        gs.TogglePhase((GamePhase)((int)(gs.phase + 1) % (int)GamePhase._Count));
        Selection.activeGameObject = gs.phaseUI[(int)gs.phase].gameObject;

    }
#endif
}
