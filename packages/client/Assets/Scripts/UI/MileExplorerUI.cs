using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MileExplorerUI : SPWindowParent
{
    [Header("Mile Details")]
    [SerializeField] SPHeading title;
    [SerializeField] SPTextScriptable incompleteMileText;
    [SerializeField] SPTextSequence mileText;
    [SerializeField] Slider slider;
    [SerializeField] Image fullscreenBG;
    [SerializeField] GameObject finishedMileControls;

    [Header("Column")]
    public MileColumn column;

    bool hasStarted = false; 
    int lastMile = -1;
    int mile = -10;
    int sliderMile = -99;

    protected override void Awake() {
        base.Awake();

        GameStateComponent.OnGameStateUpdated += SetMaxMile;
    }

    protected override void Destroy()
    {
        base.Destroy();
        GameStateComponent.OnGameStateUpdated -= SetMaxMile;
    }

    protected override void Start() {
        base.Start();
        hasStarted = true;
        Toggle(true);
    }

    protected override void OnEnable() {
        base.OnEnable();

        if(!hasStarted) { return; }
        Toggle(true);

    }

    public void SetMaxMile() {
        slider.maxValue = GameStateComponent.MILE_COUNT + 1;
        slider.minValue = 0;
    }


    void Update() {
        if(column.Mile != mile) {
            SetMile(column.Mile);
            slider.value = column.Mile;
        }
    }

    public void UpdateSlider() {
        sliderMile = Mathf.RoundToInt(slider.value);
        if(sliderMile != mile) {
            column.SetMile(sliderMile);
            SetMile(sliderMile);
        }
    }

    public void SetMile(int newMile) {

        lastMile = mile;
        mile = newMile;

        Debug.Log("Loading " + newMile);

        title.UpdateField("Mile " + (newMile+1));

        bool isComplete = GameStateComponent.MILE_COUNT <= newMile;
        if(isComplete) {
            LoadMileMetadata(newMile);
        } else {
            mileText.SetText(new SPTextScriptable[1]{incompleteMileText});
        }
    
    }

    public void LoadMileMetadata(int newMile) {
        SPTextScriptable [] texts;  
        texts = Resources.LoadAll<SPTextScriptable>("Data/Writing/Mile " + (newMile+1)); 
        mileText.SetText(texts);
        Debug.Log(texts.Length + " texts found");
    }

    public void SaveImage() {

    }

    protected override void OnDisable() {
        base.OnDisable();
        Toggle(false);

    }

    void Toggle(bool toggle) {

        column.gameObject.SetActive(toggle);
        fullscreenBG.gameObject.SetActive(toggle);

        if(toggle) {

            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(2.25f);
            SPUIBase.ToggleMotherUI(false);

        } else {
            //todo set to world scroll
            if(hasStarted) {
                SPCamera.SetFollow(null);
                SPCamera.SetTarget(Vector3.zero);
                SPCamera.SetFOVGlobal(10f);
                SPUIBase.ToggleMotherUI(true);
            }
        }

        SPCamera.ToggleScroll(!toggle);


    }


}
