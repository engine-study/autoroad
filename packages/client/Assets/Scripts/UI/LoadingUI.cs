using System.Collections;
using System.Collections.Generic;
using System.Data;
using mud;
using mud;
using Unity.VisualScripting;
using UnityEngine;

public class LoadingUI : SPWindowParent
{
    [Header("Loading")]
    [SerializeField] TMPro.TextMeshProUGUI text;
    float dotTime = 0f;
    int totalTables = -1;
    int dots;
    string dotString;

    public override void Init() {
        base.Init();

    }

    public void Toggle(bool toggle) {

        ToggleWindow(toggle);

        if(toggle) {
            text.text = "";
        }
                

    }

    protected override void OnEnable() {
        base.OnEnable();
        totalTables = -1;
        Toggle(true);
    }

    protected override void OnDisable() {
        base.OnDisable();
        Toggle(false);
    }

    void Update() {
        
        dotTime -= Time.deltaTime;
        if(dotTime < 0f) {
            dots = (dots+1) % 4;
            dotTime += .2f;
            dotString = "";
            for(int i = 0; i < dots; i++) {
                dotString += ".";
            }
        }

        if(NetworkManager.Instance.ds == null) {
            text.text = "Connecting to network" + dotString;
        } else {
            text.text = TableDictionary.Tables.Count + " tables loaded" + dotString;
        }




    }

}
