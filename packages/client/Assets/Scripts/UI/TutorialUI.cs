using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : SPWindowParent
{
    [Header("Tutorial")]
    [SerializeField] SPTextSequence tutorialText;
    [SerializeField] GameObject tutorialParent;
    [SerializeField] GameObject [] tutorials;

    protected override void Awake() {
        base.Awake();
        tutorialText.OnUpdated += ShowTutorial;
    }

    public override void ToggleWindow(bool toggle) {
        base.ToggleWindow(toggle);
        tutorialParent.SetActive(toggle);
    }

    void ShowTutorial() {
        for(int i = 0; i < tutorials.Length; i++) {
            tutorials[i].SetActive(i == tutorialText.Index);
        }
    }

}
