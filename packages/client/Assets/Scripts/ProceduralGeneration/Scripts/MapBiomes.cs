using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapBiomes", menuName = "Engine/Map/MapBiomes", order = 1)]
public class MapBiomes : ScriptableObject
{
    [Header("Map Biomes")]
   	public TerrainType[] regions;

	// void OnValidate() {
	// 	Debug.Log("Region validate");
	// }
}
