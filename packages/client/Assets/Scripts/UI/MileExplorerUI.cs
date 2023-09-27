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

    [Header("Debug")]
    [SerializeField] int mile = -10;
    [SerializeField] bool hasSetup = false; 
    
    int lastMile = -1;
    int sliderMile = -99;
    bool firstTime = true;

    public override void Init() {
        base.Init();
        slider.value = 0f;
        column.SetMile(0f);
        GameStateComponent.OnGameStateUpdated += SetMaxMile;
    }

    protected override void Destroy() {
        base.Destroy();
        GameStateComponent.OnGameStateUpdated -= SetMaxMile;
    }

    protected override void OnEnable() {
        base.OnEnable();
        if(!hasSetup) return;
        Toggle(true);
    }

    protected override void OnDisable() {
        base.OnDisable();
        Toggle(false);
    }

    public void SetMaxMile() {
        hasSetup = true;
        slider.maxValue = GameStateComponent.MILE_COUNT + .25f;
        slider.minValue = -.25f;
    }

    void Update() {

        if(column.IsInputting) {
            slider.value = Mathf.Lerp(slider.value, column.MileRaw, .25f);
        } else {
            slider.value = Mathf.Lerp(slider.value, mile, .25f);
            column.SetMile(slider.value);
            sliderMile = Mathf.RoundToInt(slider.value);
        }

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

        Debug.Log("EXPLORE " + newMile);

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

        column.gameObject.SetActive(toggle);
        fullscreenBG.gameObject.SetActive(toggle);

        if(toggle) {

            if(firstTime) {
                SetMaxMile();
                SetMile(0);
            }

            firstTime = false;

            column.SetRot(mile - .25f);
            SetMile(mile);

            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(2f);

        } else {
            //todo set to world scroll
            if(hasInit) {
                SPCamera.SetFollow(null);
                SPCamera.SetTarget(Vector3.zero);
                SPCamera.SetFOVGlobal(10f);
            }
        }

        if(hasInit) {
            SPUIBase.ToggleMotherUI(!toggle);
        }

        SPCamera.ToggleScroll(!toggle);


    }


}
