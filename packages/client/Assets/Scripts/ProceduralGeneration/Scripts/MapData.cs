using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Engine/Map/MapData", order = 1)]
public class MapData : ScriptableObject
{
	public enum MapGen {Noise, Fill};

    [Header("Map Data")]
    public int mapWidth;
	public int mapHeight;

	public MapGen genType;
	public float mapFill = .5f;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;
	public float falloffStrength = 0f; 
	public float blendStrength = 1f; 

    // void OnValidate() {
	// 	Debug.Log("Data validate");
	// }

}
