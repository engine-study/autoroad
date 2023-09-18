using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : SPWindowParent
{
    [Header("Tutorial")]
    [SerializeField] SPTextSequence tutorialText;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] Transform tutorialTransform;
    [SerializeField] GameObject [] tutorials;
    float vertical = 1f;
    bool hasStarted = false;
    Vector3 targetPos;

    protected override void Awake() {
        base.Awake();
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
            tutorialTransform.localPosition = targetPos;
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(tutorialParent.transform.position + Vector3.up * vertical);
            SPCamera.SetFOVGlobal(5f);

        } else {
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(Vector3.zero);
            SPCamera.SetFOVGlobal(10f);
        }

        SPCamera.ToggleScroll(!toggle);


    }

    void Update() {
        tutorialTransform.localPosition = Vector3.Lerp(tutorialTransform.localPosition, targetPos, .05f);
    }

    void ShowTutorial() {

        targetPos = Vector3.left * tutorialText.Index * 10f;

        // for(int i = 0; i < tutorials.Length; i++) {
        //     tutorials[i].SetActive(i == tutorialText.Index);
        // }
    }

}
