using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapSave", menuName = "Engine/Map/MapSave", order = 1)]
public class MapSave : ScriptableObject
{   

    public Dictionary<Vector2, float> noiseMap;
    public Dictionary<Vector2, float> biomeMap;
    public Dictionary<Vector2, float> falloffMap;
    public Dictionary<Vector2, float> blendMap;
    public Color[] colourMap;
    public Dictionary<Vector2, Ground> blocks;
    public Dictionary<Vector2, Entity> entities;

}