using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud.Client;
using IWorld.ContractDefinition;
using Cysharp.Threading.Tasks;

public class MotherUI : SPUIInstance {

    public static MotherUI Mother;
    public static SPActionWheelUI ActionWheel {get { return Mother.wheel; } }

    [Header("UI")]
    public GameObject loadingScreen;
    public Image loadingScreenBackground;
    public SPActionWheelUI wheel;
    public NameOptionUI playerCreate;
    public SpawningUI spawning;

    [Header("Menu")]
    public MenuUI menu;
    public DebugUI debug;
    public WorldUI playerInfo;

    [Header("Store")]
    public StoreUI store;

    [Header("Game")]
    public GameUI game;
    public AudioClip sfx_spawn;
    public AudioClip sfx_start;

    List<SPWindowParent> gameMenus = new List<SPWindowParent>();

    protected override void Awake() {
        base.Awake();

        Mother = this;

        gameMenus.Add(store);
        gameMenus.Add(debug);
        gameMenus.Add(menu);

        foreach(SPWindowParent w in gameMenus) {
            w.ToggleWindowClose();
        }

        ToggleLoading(true);
        TogglePlayerCreation(false);
        TogglePlayerSpawning(false);

        ToggleGame(false);

        SPEvents.OnServerLoaded += ShowServer;
        SPEvents.OnLocalPlayerSpawn += SpawnPlayer;

    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnServerLoaded -= ShowServer;
        SPEvents.OnLocalPlayerSpawn -= SpawnPlayer;

        TxManager.OnTransaction -= UpdateWheel;
        TxUpdate.OnUpdate -= UpdateWheelOptimistic;

    }


    void UpdateWheelOptimistic(TxUpdate update) {
        if (update.Info.UpdateSource == UpdateSource.Optimistic) {
            wheel.UpdateState(ActionEndState.Success, true);
        }
    }

    void UpdateWheel(bool txSuccess) {

        if (txSuccess) {

        } else {
            wheel.UpdateState(ActionEndState.Failed, true);
        }
    }

    public static void ToggleLoading(bool toggle) {
        Mother.loadingScreen.SetActive(toggle);
    }
    public static void TogglePlayerCreation(bool toggle) {
        Mother.playerCreate.ToggleWindow(toggle);
    }

    public static void TogglePlayerSpawning(bool toggle) {
        Mother.spawning.ToggleWindow(toggle);
    }

    public static void ToggleGame(bool toggle) {
        Mother.playerInfo.ToggleWindow(!toggle);
        Mother.game.ToggleWindow(toggle);
    }
    
    public void ToggleMenuWindow(SPWindowParent open) {
        open.ToggleWindow();
        foreach(SPWindowParent w in gameMenus) {
            if(w != open) {
                w.ToggleWindowClose();
            }
        }
    }

    void SpawnPlayer() {

        ToggleGame(true);

        TxManager.OnTransaction += UpdateWheel;
        TxUpdate.OnUpdate += UpdateWheelOptimistic;

        SPUIBase.PlaySound(sfx_spawn);

        SPCamera.SetFollow(SPPlayer.LocalPlayer.Root);
        SPCamera.SetFOVGlobal(5f);

    }


    void ShowServer() {
        StartCoroutine(ServerCoroutine());
    }

    IEnumerator ServerCoroutine() {

        //play a sound

        //animate out screen 
        loadingScreenBackground.color = Color.black - Color.black;
        yield return new WaitForSeconds(.1f);

        SPUIBase.PlaySound(sfx_start);

        //hide loading
        ToggleLoading(false);

    }


}
