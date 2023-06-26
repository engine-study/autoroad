using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GamePhase {Lobby, Build, Action, PostGame,_Count}
public class GameState : MonoBehaviour
{
    public static GamePhase Phase {get{return Instance.phase;}}
    public static GameState Instance;
    public GamePhase phase;

    [Header("UI")]
    public GameUI gameUI;
    public PhaseUI [] phaseUI;
    public static System.Action<GamePhase> OnPhaseUpdate;

    void Awake() {
        Instance = this;
        phase = GamePhase.Lobby;
    }

    void Start() {

    }

    void UpdatePhase() {
        phaseUI[(int)phase].UpdatePhase();
    }

    public void TogglePhase(GamePhase newPhase) {

        phaseUI[(int)phase].ToggleWindow(false);

        if(phase == GamePhase.Lobby) {

        } else if(phase == GamePhase.Build) {

        } else if(phase == GamePhase.Action) {

        } else if(phase == GamePhase.PostGame) {

        }

        phase = newPhase;

        gameUI.UpdatePhase(phase);
        phaseUI[(int)phase].ToggleWindow(true);

        if(phase == GamePhase.Lobby) {

        } else if(phase == GamePhase.Build) {

        } else if(phase == GamePhase.Action) {

        } else if(phase == GamePhase.PostGame) {

        }

    }

    void OnDestroy() {
        Instance = null;
    }

    #if UNITY_EDITOR
    [MenuItem("Engine/Next Phase &q")]
    static void TogglePhase()
    {
        GameState gs = FindObjectOfType<GameState>();

        if(!gs) {return;}

        gs.TogglePhase((GamePhase)((int)(gs.phase + 1)%(int)GamePhase._Count));
        Selection.activeGameObject = gs.phaseUI[(int)gs.phase].gameObject;

    }
    #endif
}
