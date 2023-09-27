using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MileInfo", menuName = "Gaul/MileInfo", order = 1)]
public class GaulMile : ScriptableObject {

    public int Mile = -1;
    public int Prompt;
    public int Description;
    [SerializeField] List<SPTextScriptable> texts;
    [SerializeField] Texture2D image;
}
