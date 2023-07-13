using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mud.Client;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GamePhase { Lobby, Game, PostGame, _Count }
public class GameState : MonoBehaviour {
    public static GamePhase Phase { get { return Instance.phase; } }
    public static GameState Instance;
    public GamePhase phase;
    public RecieverMUD reciever;
    public PlayerMUD localPlayer;

    [Header("Scroll")]
    public WorldScroll scroll;

    [Header("Debug")]
    public GameObject editorObjects;

    [Header("UI")]
    public PhaseUI[] phaseUI;
    public static System.Action<GamePhase> OnPhaseUpdate;

    void Awake() {
        Instance = this;
        editorObjects.SetActive(false);

        SPEvents.OnLocalPlayerSpawn += SetupPlayer;
    }

    void SetupPlayer() {
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
        if(SPUIBase.CanInput && Input.GetMouseButtonDown(0)) {
            if(CursorMUD.Base) {
                SPCamera.SetFollow(CursorMUD.Base.Root);
            } else {
                SPCamera.SetFollow(null);
                float x = Mathf.Clamp(SPInput.MousePlanePos.x, -10f, 10f);
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

    void OnDestroy() {
        Instance = null;
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
