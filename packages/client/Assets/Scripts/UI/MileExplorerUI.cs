using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MileExplorerUI : SPWindowParent
{
    [Header("Mile Details")]
    [SerializeField] SPHeading title;
    [SerializeField] SPTextScriptable unknownMileText;
    [SerializeField] SPTextScriptable incompleteMileText;
    [SerializeField] SPTextSequence mileText;
    [SerializeField] Slider slider;
    [SerializeField] Image fullscreenBG;
    [SerializeField] GameObject finishedMileControls;

    [Header("Column")]
    public MileColumn column;

    bool hasSetup = false; 
    int lastMile = -1;
    int mile = -10;
    int sliderMile = -99;
    bool firstTime = true;

    public override void Init() {
        base.Init();

        SetMaxMile();
        GameStateComponent.OnGameStateUpdated += SetMaxMile;
    }

    protected override void Destroy() {
        base.Destroy();
        GameStateComponent.OnGameStateUpdated -= SetMaxMile;
    }

    protected override void OnEnable() {
        base.OnEnable();
        Toggle(true);
    }

    protected override void OnDisable() {
        base.OnDisable();
        Toggle(false);
    }

    public void SetMaxMile() {
        hasSetup = true;
        slider.maxValue = GameStateComponent.MILE_COUNT + 1.45f;
        slider.minValue = -.45f;
    }

    void Update() {

        if(Input.GetMouseButton(0)) {

        } else {
            slider.value = Mathf.Lerp(slider.value, mile, .2f);
        }

        //rotate column to slider value;
        column.SetMile(slider.value);
        sliderMile = Mathf.RoundToInt(slider.value);
        
        // if(sliderMile != mile) {
        //     SetMile(sliderMile);
        // }

        if(column.Mile != mile) {
            SetMile(column.Mile);
        }

        if(Input.GetMouseButtonUp(0)) {
            slider.OnDeselect(null);
        }
    }

    public void SetMile(int newMile) {

        lastMile = mile;
        mile = newMile;

        Debug.Log("Loading " + newMile);

        title.UpdateField("Mile " + (newMile+1));

        ChunkComponent chunk = ChunkComponent.GetChunk(newMile);
        
        if(chunk == null || !chunk.Completed) {
            mileText.ToggleWindowClose();
        } else {
            LoadMileMetadata(newMile);
        } 
    
    }

    public void LoadMileMetadata(int newMile) {

        SPTextScriptable [] texts;  
        texts = Resources.LoadAll<SPTextScriptable>("Data/Writing/Mile " + (newMile+1)); 
        mileText.SetText(texts);
        Debug.Log(texts.Length + " texts found");

        mileText.ToggleWindowOpen();

    }

    public void SaveImage() {

    }


    void Toggle(bool toggle) {

        if(!hasSetup) return;

        SetMaxMile();

        column.gameObject.SetActive(toggle);
        fullscreenBG.gameObject.SetActive(toggle);

        if(toggle) {

            if(firstTime) {SetMile(0);}

            firstTime = false;

            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(2f);

        } else {
            //todo set to world scroll
            if(hasSetup) {
                SPCamera.SetFollow(null);
                SPCamera.SetTarget(Vector3.zero);
                SPCamera.SetFOVGlobal(10f);
            }
        }

        if(hasSetup) {
            SPUIBase.ToggleMotherUI(!toggle);
        }

        SPCamera.ToggleScroll(!toggle);


    }


}
