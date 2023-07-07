using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherUI : SPUIInstance
{

    [Header("UI")]
    public GameObject loadingScreen;
    public Image loadingScreenBackground;

    protected override void Awake()
    {
        base.Awake();

        loadingScreen.SetActive(true);

        SPEvents.OnLocalPlayerSpawn += StartGame;
    }
    protected override void Start() {
        base.Start();


    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SPEvents.OnLocalPlayerSpawn -= StartGame;
    }

    void StartGame() {
        StartCoroutine(StartCoroutine());
    }
    IEnumerator StartCoroutine() {

        //play a sound
        SPEvents.OnLocalPlayerSpawn -= StartGame;

        if(!SPGlobal.IsDebug) {
            yield return new WaitForSeconds(1f);
        }

        float lerp = 0f;

        while(lerp < 1f) {
            lerp += Time.deltaTime;
            loadingScreenBackground.color = Color.white - Color.black * lerp;
            yield return null;
        }

        loadingScreen.SetActive(false);

        //show the game!


    }


}
