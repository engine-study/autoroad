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
    public bool freshInstall;
    public GameObject loadingScreen;
    public Image loadingScreenBackground;
    public SPActionWheelUI wheel;
    public NameOptionUI playerCreate;
    public SpawningUI spawning;
    public RespawnUI respawn;
    public TutorialUI tutorial;

    [Header("Menu")]
    public MainMenuUI menu;
    public SpectateUI spectate;

    [Header("Game")]
    public GameUI game;
    public MapUI map;
    public StoreUI store;
    public SPWindow debugButton;
    public DebugUI debug;
    public WorldUI playerInfo;
    public ProfileUI profile;
    public AudioClip sfx_spawn;
    public AudioClip sfx_start;


    protected override void Awake() {
        base.Awake();

        SPGlobal.OnDebug += OnDebug;
        OnDebug(SPGlobal.IsDebug);

        Mother = this;

        profile.ToggleWindowClose();
        map.ToggleWindowClose();
        menu.ToggleWindowClose();
        spectate.ToggleWindowClose();

        ToggleLoading(true);
        TogglePlayerCreation(false);
        TogglePlayerSpawning(false);
        ToggleRespawn(false);

        ToggleGame(false);

        SPEvents.OnServerLoaded += ShowServer;
        SPEvents.OnGameStarted += PlayGame;

    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnServerLoaded -= ShowServer;
        SPEvents.OnGameStarted -= PlayGame;

        TxManager.OnTransaction -= UpdateWheel;
        TxUpdate.OnUpdated -= UpdateWheelOptimistic;

    }

    protected override void Start() {
        base.Start();
                
        tutorial.ToggleWindowClose();

    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.BackQuote) && Input.GetKey(KeyCode.LeftShift)) {
            SPGlobal.ToggleDebug(!SPGlobal.IsDebug);
        }
    }

    void OnDebug(bool toggle) {
        debugButton.ToggleWindow(toggle);
    }



    void UpdateWheelOptimistic(TxUpdate update) {
        if (update.Info.Source == UpdateSource.Optimistic) {
            wheel.UpdateState(ActionEndState.Success, true);
        }
    }

    public static void TransactionFailed() {
        SPCamera.AddShakeGlobal(.1f);
        ActionWheel.UpdateState(ActionEndState.Failed, true);
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


    public static void ToggleRespawn(bool toggle) {
        Mother.respawn.ToggleWindow(toggle);
    }

    public static void TogglePlayerSpawning(bool toggle) {
        Mother.spawning.ToggleWindow(toggle);
    }

    public static void ToggleGame(bool toggle) {
        Mother.playerInfo.ToggleWindow(!toggle);
        Mother.game.ToggleWindow(toggle);
    }

    void PlayGame() {

        ToggleGame(true);

        TxManager.OnTransaction += UpdateWheel;
        TxUpdate.OnUpdated += UpdateWheelOptimistic;

        SPUIBase.PlaySound(sfx_spawn);

        FollowPlayer();

        Debug.Log("Play Game", this);

    }

    public static void FollowPlayer() {
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

        map.ToggleWindowOpen();
        menu.ToggleWindowOpen();

        Debug.Log("Main Menu", this);

    }


}
