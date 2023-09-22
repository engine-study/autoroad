using System.Collections;
using System.Collections.Generic;
using mud.Client;
using mud.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI text;

    float dotTime = 0f;
    int dots;
    string dotString;

    void OnEnable() {
        text.text = "";
    }

    void Update() {
        
        dotTime -= Time.deltaTime;
        if(dotTime < 0f) {
            dots = (dots+1) % 3;
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

    void OnDisable() {

    }
}
