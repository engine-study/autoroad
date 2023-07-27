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

        playerTable.OnInit += SpawnLocalPlayer;
        SPEvents.OnLocalPlayerSpawn += RecieverPlayer;
    }

    void OnDestroy() {
        playerTable.OnInit -= SpawnLocalPlayer;
        SPEvents.OnLocalPlayerSpawn -= RecieverPlayer;
        Instance = null;
    }

    void SpawnLocalPlayer() {

        var addressKey = NetworkManager.Instance.addressKey;
        var currentPlayer = IMudTable.GetValueFromTable<PlayerTable>(addressKey);

        if (currentPlayer == null) {
            // spawn the player
            Debug.Log("Go to player creation");
            SetupPlayerCreation();
        } else {
            Debug.Log("Found spawned player");
            StartGame();
        }

    }



    void SetupPlayerCreation() {
        MotherUI.TogglePlayerCreation(true);
    }

    void StartGame() {

    }


    //player is spawned
    void RecieverPlayer() {
        SPPlayer.LocalPlayer.SetReciever(reciever);
        localPlayer = SPPlayer.LocalPlayer as PlayerMUD; 
    }


    void Start() {
        for (int i = 0; i < phaseUI.Length; i++) {
            phaseUI[i].ToggleWindowClose();
        }

        TogglePhase(phase);
    }

    void Update() {
        UpdateInput();
    }

    void UpdateInput() {
        if(SPUIBase.CanInput && Input.GetMouseButtonDown(1)) {
            if(CursorMUD.Base) {
                SPCamera.SetFollow(CursorMUD.Base.Root);
            } else {
                SPCamera.SetFollow(null);
                float x = Mathf.Clamp(SPInput.MousePlanePos.x, BoundsComponent.Left, BoundsComponent.Right);
                float z = Mathf.Clamp(SPInput.MousePlanePos.z, 0f, GameStateComponent.MILE_ENDPOS);
                SPCamera.SetTarget(new Vector3(x, 0f, z));
            }
            
        }

        if(SPUIBase.CanInput && Input.GetKeyDown(KeyCode.Space)) {
            if(localPlayer) {
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
