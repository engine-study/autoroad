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
    public MenuUI menu;
    public GameObject menusParent;
    public SPWindow debugButton;
    public DebugUI debug;
    public WorldUI playerInfo;
    [Header("Profile")]
    public ProfileUI profile;

    [Header("Store")]
    public StoreUI store;

    [Header("Game")]
    public GameUI game;
    public AudioClip sfx_spawn;
    public AudioClip sfx_start;

    public List<SPWindowParent> gameMenus;

    protected override void Awake() {
        base.Awake();

        SPGlobal.OnDebug += OnDebug;
        OnDebug(SPGlobal.IsDebug);

        Mother = this;

        gameMenus = new List<SPWindowParent>();
        for(int i = 0; i < menusParent.transform.childCount; i++) {
            SPWindowParent menuWindow = menusParent.transform.GetChild(i).GetComponent<SPWindowParent>();
            if(menuWindow != null) gameMenus.Add(menuWindow);
        }
        
        foreach(SPWindowParent w in gameMenus) {
            w.ToggleWindowClose();
        }

        profile.ToggleWindowClose();

        ToggleLoading(true);
        TogglePlayerCreation(false);
        TogglePlayerSpawning(false);
        ToggleRespawn(false);

        ToggleGame(false);

        SPEvents.OnServerLoaded += ShowServer;
        SPEvents.OnLocalPlayerSpawn += SpawnPlayer;

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

    protected override void OnDestroy() {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnServerLoaded -= ShowServer;
        SPEvents.OnLocalPlayerSpawn -= SpawnPlayer;

        TxManager.OnTransaction -= UpdateWheel;
        TxUpdate.OnUpdated -= UpdateWheelOptimistic;

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
        TxUpdate.OnUpdated += UpdateWheelOptimistic;

        SPUIBase.PlaySound(sfx_spawn);

        FollowPlayer();

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

    }


}
