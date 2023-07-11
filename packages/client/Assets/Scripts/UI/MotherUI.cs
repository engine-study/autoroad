using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherUI : SPUIInstance
{

    [Header("UI")]
    public GameObject loadingScreen;
    public Image loadingScreenBackground;

    [Header("Game")]
    public GameUI game;

    protected override void Awake()
    {
        base.Awake();

        loadingScreen.SetActive(true);

        SPEvents.OnLocalPlayerSpawn += StartGame;
    }
    protected override void Start()
    {
        base.Start();


    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SPEvents.OnLocalPlayerSpawn -= StartGame;
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
            yield return new WaitForSeconds(1f);
            float lerp = 0f;

            while (lerp < 1f)
            {
                lerp += Time.deltaTime;
                loadingScreenBackground.color = Color.white - Color.black * lerp;
                yield return null;
            }
            
            //another sound
            yield return new WaitForSeconds(.5f);

        }

        //hide loading
        loadingScreen.SetActive(false);

        //show the game!


    }


}
