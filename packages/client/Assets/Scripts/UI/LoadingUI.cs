using System.Collections;
using System.Collections.Generic;
using System.Data;
using mud;
using mud;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

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

        if(NetworkManager.Initialized == false) {
            text.text = "Connecting to network" + dotString;
        } else {
            // text.text = TableDictionary.Tables.Count + " tables loaded" + dotString;
            string loadText = "";
            if(TableManager.LatestTable) {
                loadText =  TableManager.LatestTable.ToString().Substring(0, TableManager.LatestTable.ToString().IndexOf("Table")) + " Table";
            }
            // for(int i = 0; i < TableDictionary.Tables.Count; i++) {
            //     if(TableDictionary.Tables[i].Loaded == false) {
            //         loadText = TableDictionary.Tables.Count > 0 ? TableDictionary.Tables[0].ToString().Substring(0,TableDictionary.Tables[0].ToString().IndexOf("Table")) + " Table" : "";
            //         break;
            //     }
            // }

            text.text = $"Loading {loadText}{dotString}";
        }




    }

}
