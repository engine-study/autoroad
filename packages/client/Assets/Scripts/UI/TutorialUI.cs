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
    float vertical = 1f;
    float distance = 25f;
    bool hasStarted = false;
    Vector3 targetPos;

    protected override void Awake() {
        base.Awake();

        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].transform.localPosition = Vector3.zero;
        }

        tutorialText.OnUpdated += ShowTutorial;
    }

    protected override void Start() {
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

    void Toggle(bool toggle) {

        tutorialParent.SetActive(toggle);

        if(toggle) {

            SPAudioSource.PlayGlobal(startClip);
            tutorialTransform.localPosition = targetPos;
            SPCamera.SetFollow(null);
            SPCamera.SetFOVGlobal(5f);
            SPCamera.SetTarget(tutorialParent.transform.position); //+ Vector3.right * tutorialText.Index * distance + Vector3.down * vertical
            SPUIBase.ToggleMotherUI(false);
            ShowTutorial();

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

        OnTutorial?.Invoke(toggle);

    }

    void ShowTutorial() {


        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].SetActive(i == tutorialText.Index);
        }
    }

}
