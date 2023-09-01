using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionData : MonoBehaviour
{
    [Header("Actions")]
    [EnumNamedArray( typeof(ActionName) )]
    public ActionEffect [] effects;

    void Awake() {
        for (int i = 0; i < effects.Length; i++) {
            if(effects[i] == null) continue;
            effects[i].gameObject.SetActive(false);
        }
    }
}
