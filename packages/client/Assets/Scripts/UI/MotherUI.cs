using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using mud.Client;
public class MotherUI : SPUIInstance
{

    public static MotherUI Mother;

    [Header("UI")]
    public GameObject loadingScreen;
    public Image loadingScreenBackground;
    public SPActionWheelUI wheel;


    [Header("Game")]
    public GameUI game;

    protected override void Awake()
    {
        base.Awake();

        Mother = this;

        loadingScreen.SetActive(true);

        SPEvents.OnLocalPlayerSpawn += StartGame;
        TxManager.OnTransaction += UpdateWheel;
    }
    protected override void Start()
    {
        base.Start();


    }

    void UpdateWheel(bool txSuccess) {
        if(txSuccess) {

        } else {
            wheel.UpdateState(ActionEndState.Failed, true);
        }
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();

        Mother = null;

        SPEvents.OnLocalPlayerSpawn -= StartGame;
        TxManager.OnTransaction -= UpdateWheel;

    }

    void StartGame()
    {
        StartCoroutine(StartCoroutine());
    }
    IEnumerator StartCoroutine()
    {

        //play a sound
        SPEvents.OnLocalPlayerSpawn -= StartGame;

        //animate out screen 
        if (!SPGlobal.IsDebug)
        {
            yield return new WaitForSeconds(2f);
            float lerp = 0f;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime;
                loadingScreenBackground.color = Color.white - Color.black * lerp;
                yield return null;
            }
            
            //another sound
            yield return new WaitForSeconds(1f);

        }

        //hide loading
        loadingScreen.SetActive(false);

        //show the game!


    }


}
