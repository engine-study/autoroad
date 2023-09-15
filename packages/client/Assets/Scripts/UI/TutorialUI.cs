using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : SPWindowParent
{
    [Header("Tutorial")]
    [SerializeField] SPTextSequence tutorialText;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] GameObject [] tutorials;
    float vertical = 1f;
    bool hasStarted = false;

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

    void Toggle(bool toggle) {

        tutorialParent.SetActive(toggle);

        if(toggle) {
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(tutorialParent.transform.position + Vector3.up * vertical);
            SPCamera.SetFOVGlobal(2f);

        } else {
            SPCamera.SetFollow(null);
            SPCamera.SetTarget(Vector3.zero);
            SPCamera.SetFOVGlobal(10f);
        }

    }

    void ShowTutorial() {
        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].SetActive(i == tutorialText.Index);
        }
    }

}
