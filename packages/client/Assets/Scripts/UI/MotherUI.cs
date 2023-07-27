using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud.Client;
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

        SPEvents.OnLocalPlayerSpawn += StartGame;
        TxManager.OnTransaction += UpdateWheel;
    }
    protected override void Start() {
        base.Start();


    }

    void UpdateWheel(bool txSuccess) {
        if (txSuccess) {

        } else {
            wheel.UpdateState(ActionEndState.Failed, true);
        }
    }

    protected override void OnDestroy() {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnLocalPlayerSpawn -= StartGame;
        TxManager.OnTransaction -= UpdateWheel;

    }

    public static void ToggleLoading(bool toggle) {
        Mother.loadingScreen.SetActive(toggle);
    }
    public static void TogglePlayerCreation(bool toggle) {
        Mother.playerCreate.gameObject.SetActive(toggle);
    }

    void StartGame() {
        StartCoroutine(StartCoroutine());
    }


    IEnumerator StartCoroutine() {

        //play a sound
        SPEvents.OnLocalPlayerSpawn -= StartGame;

        SPUIBase.PlaySound(sfx_spawn);

        //animate out screen 
        if (!SPGlobal.IsDebug) {

            yield return new WaitForSeconds(1f);
            loadingScreenBackground.color = Color.black - Color.black;
            yield return new WaitForSeconds(1f);

        }

        SPUIBase.PlaySound(sfx_start);

        //hide loading
        loadingScreen.SetActive(false);

        //show the game!


    }


}
