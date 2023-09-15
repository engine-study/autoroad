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

    protected override void Awake() {
        base.Awake();
        tutorialText.OnUpdated += ShowTutorial;
    }

    protected override void OnEnable() {
        base.OnEnable();
        SPCamera.SetFollow(null);
        SPCamera.SetTarget(tutorialParent.transform.position + Vector3.up * vertical);
        SPCamera.SetFOVGlobal(2f);

        tutorialParent.SetActive(true);

    }

    protected override void OnDisable() {
        base.OnDisable();

        SPCamera.SetFollow(null);
        SPCamera.SetTarget(Vector3.zero);
        SPCamera.SetFOVGlobal(10f);

        tutorialParent.SetActive(false);

    }

    void ShowTutorial() {
        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].SetActive(i == tutorialText.Index);
        }
    }

}
