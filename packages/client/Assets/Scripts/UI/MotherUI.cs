using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud.Client;
using IWorld.ContractDefinition;

public class MotherUI : SPUIInstance {

    public static MotherUI Mother;

    [Header("UI")]
    public GameObject loadingScreen;
    public Image loadingScreenBackground;
    public SPActionWheelUI wheel;
    public NameOptionUI playerCreate;

    [Header("Game")]
    public GameUI game;
    public AudioClip sfx_spawn;
    public AudioClip sfx_start;

    protected override void Awake() {
        base.Awake();

        Mother = this;

        ToggleLoading(true);
        TogglePlayerCreation(false);

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
        Mother.playerCreate.gameObject.SetActive(toggle);
    }

    void SpawnPlayer() {

        TxManager.OnTransaction += UpdateWheel;
        TxUpdate.OnUpdate += UpdateWheelOptimistic;
        SPUIBase.PlaySound(sfx_spawn);
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
