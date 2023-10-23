using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : SPWindowParent
{
    public static System.Action<bool> OnTutorial;

    [Header("Tutorial")]
    [SerializeField] SPTextSequence tutorialText;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] Transform tutorialTransform;
    [SerializeField] GameObject [] tutorials;
    [SerializeField] AudioClip startClip, endClip;
    [SerializeField] SPButton complete;

    [Header("Debug")]
    public bool hasCompleted = false;
    public bool hasStarted = false;

    float vertical = 1f;
    float distance = 25f;
    Vector3 targetPos;
    string tutorial = "tutorial";
    public override void Init() {

        if(HasInit) {return;}
        base.Init();

        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].transform.localPosition = Vector3.zero;
        }

        hasCompleted = false;
        var completed = PlayerPrefs.GetString(tutorial);

        if (!string.IsNullOrWhiteSpace(completed)){
            hasCompleted = true;
        }

        tutorialText.OnUpdated += ShowTutorial;
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

    protected override void OnDisable() {
        base.OnDisable();

        Toggle(false);

    }

    public override void ToggleWindow(bool toggle) {
        base.ToggleWindow(toggle);
        Toggle(toggle);
    }

    void Update() {

        complete.ToggleWindow(tutorialText.HasReachedFinal);
        
        if(Input.GetKeyDown(KeyCode.Escape)) {
            CompleteTutorial();
        }
    }

    void Toggle(bool toggle) {

        complete.ToggleWindowClose();
        tutorialParent.SetActive(toggle);

        if(!hasStarted) return;

        if(toggle) {

            SPAudioSource.PlayGlobal(startClip);
            tutorialTransform.localPosition = targetPos;
            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(4f);
            SPCamera.SetTarget(tutorialParent.transform.position); //+ Vector3.right * tutorialText.Index * distance + Vector3.down * vertical
            SPUIBase.ToggleMotherUI(false);
            ShowTutorial();

        } else {
            //todo set to world scroll
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(Vector3.zero);
            SPCamera.SetFOVGlobal(10f);
            SPUIBase.ToggleMotherUI(true);
            
        }

        CameraControls.ToggleZoom(!toggle);
        CameraControls.TogglePan(!toggle);

        OnTutorial?.Invoke(toggle);

    }

    public void CompleteTutorial() {
        
        PlayerPrefs.SetString(tutorial, "true");
        PlayerPrefs.Save();
        ToggleWindow(false);
    }

    void ShowTutorial() {

        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].SetActive(i == tutorialText.Index);
        }
    }

}
