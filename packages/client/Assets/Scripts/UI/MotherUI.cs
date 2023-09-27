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

    [Header("Options")]
    [SerializeField] bool fastStart = false; 

    [Header("UI")]
    public LoadingUI loadingScreen;
    public GameObject nameAndSpawnScreen;
    public SPActionWheelUI wheel;
    public NameOptionUI playerCreate;
    public SpawningUI spawning;
    public RespawnUI respawn;
    public TutorialUI tutorial;
    public MileExplorerUI explorer;

    [Header("Menu")]
    public MainMenuUI menu;
    public SpectateUI spectate;

    [Header("Game")]
    public GameUI game;
    public MapUI map;
    public StoreUI store;
    public SPWindow debugButton;
    public DebugUI debug;
    public PlayerUI playerUI;
    public WorldUI playerInfo;
    public ProfileUI profile;

    [Header("Game State")]
    public AudioClip sfx_mainMenu;
    public AudioClip sfx_loaded;
    public AudioClip sfx_txSent;
    public AudioClip sfx_txRevert;


    protected override void Awake() {
        base.Awake();

        SPGlobal.OnDebug += OnDebug;
        OnDebug(SPGlobal.IsDebug);

        Mother = this;
        nameAndSpawnScreen.SetActive(false);

        profile.ToggleWindowClose();
        map.ToggleWindowClose();
        menu.ToggleWindowClose();
        spectate.ToggleWindowClose();

        explorer.Init();
        explorer.ToggleWindowClose();

        wheel.Init();

        ToggleLoading(true);
        TogglePlayerCreation(false);
        TogglePlayerSpawning(false);
        ToggleRespawn(false);

        ToggleGame(false);
        playerUI.Init();
        
        SPEvents.OnServerLoaded += ShowMainMenu;
        SPEvents.OnGameReady += GameReady;
        SPEvents.OnPlayGame += PlayGame;

    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnServerLoaded -= ShowMainMenu;
        SPEvents.OnGameReady -= GameReady;
        SPEvents.OnPlayGame -= PlayGame;

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
            wheel.ActionPending();
            SPUIBase.PlaySound(Mother.sfx_txSent);
        }
    }

    public static void TransactionSuccess() {
        ActionWheel.ActionRelease(ActionEndState.Success, true);
    }

    public static void TransactionFailed() {
        SPCamera.AddShakeGlobal(.1f);
        ActionWheel.ActionRelease(ActionEndState.Failed, true);
        SPUIBase.PlaySound(Mother.sfx_txRevert);
    }

    void UpdateWheel(bool txSuccess) {

        if (txSuccess) {
            TransactionSuccess();
        } else {
            TransactionFailed();
        }
    }

    public static void ToggleLoading(bool toggle) {
        Mother.loadingScreen.Toggle(toggle);
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
        nameAndSpawnScreen.SetActive(true);
    }
    

    public void GameReady() {

        TxManager.OnTransaction += UpdateWheel;
        TxUpdate.OnUpdated += UpdateWheelOptimistic;
        Debug.Log("[UI] Game Ready", this);

    }

    public void StartPlaying() {
        ToggleGame(true);
        WorldScroll.Instance.SetToPlayerMile();
    }

    public static void FollowPlayer() {
        SPCamera.SetFollow(SPPlayer.LocalPlayer.Root);
        SPCamera.SetFOVGlobal(5f);
    }

    void ShowMainMenu() {
        StartCoroutine(MainMenuCoroutine());
    }

    IEnumerator MainMenuCoroutine() {

        if(fastStart) {
            map.ToggleWindowOpen();
            menu.ToggleWindowOpen();
            ToggleLoading(false);

            GameState.PlayGame();
            menu.ToggleWindowClose();
            yield break;
        }

        //play a sound
        SPUIBase.PlaySound(sfx_loaded);
        ToggleLoading(false);

        yield return new WaitForSeconds(2f);

        SPUIBase.PlaySound(sfx_mainMenu);
        map.ToggleWindowOpen();
        menu.ToggleWindowOpen();

        Debug.Log("Main Menu", this);

    }


}
