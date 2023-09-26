using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MileDetails : SPWindowParent
{
    [Header("Mile Details")]
    public MileColumn column;
    [SerializeField] Image fullscreenBG;
    bool hasStarted = false; 

    protected override void Awake() {
        base.Awake();

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
