using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum GenerationStyle {OnUniformCircle, InUniformCircle, RandomWalk, InRandomCircle, OnRandomCircle,}
[CreateAssetMenu(fileName = "RegionData", menuName = "Engine/Map/RegionData", order = 1)]
public class RegionData : ScriptableObject
{
    [Header("Region Data")]
    public GenerationStyle genStyle;
    public string regionName;
    public float size = 1f;
    public RegionSpawner [] spawners;
    
	void OnValidate() {
        if (size % 2 == 0) {
			Debug.LogError("Size must be odd");
			size += 1;
		}

    }

}

[System.Serializable]
public class RegionSpawner {
    public RegionPatch [] spawns;
    public float radius = 9f;

}

[System.Serializable]
public class RegionPatch {
    public int number = 1;
    public GameObject [] prefabs;
}
