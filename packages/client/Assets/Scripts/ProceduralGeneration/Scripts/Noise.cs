using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Noise {

	public static Dictionary<Vector2, float> GenerateFillMap(MapData mapData) {

		Dictionary<Vector2, float> noiseMap = new Dictionary<Vector2, float>();

		  for (int y = (int)(mapData.mapHeight * -.5f); y < mapData.mapHeight * .5f; y++)
        {
            for (int x = (int)(mapData.mapWidth * -.5f); x < mapData.mapWidth * .5f; x++)
            {
				noiseMap [new Vector2(x, y)] = mapData.mapFill;
			}
		}

		return noiseMap;
	}

	public static Dictionary<Vector2, float> GenerateNoiseMap(MapData mapData) {

		Dictionary<Vector2, float> noiseMap = new Dictionary<Vector2, float>();

		System.Random prng = new System.Random (mapData.seed);
		Vector2[] octaveOffsets = new Vector2[mapData.octaves];
		for (int i = 0; i < mapData.octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + mapData.offset.x;
			float offsetY = prng.Next (-100000, 100000) + mapData.offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}

		if (mapData.noiseScale <= 0) {
			mapData.noiseScale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapData.mapWidth / 2f;
		float halfHeight = mapData.mapHeight / 2f;


		for (int y = (int)(mapData.mapHeight * -.5f); y < mapData.mapHeight * .5f; y++)
        {
            for (int x = (int)(mapData.mapWidth * -.5f); x < mapData.mapWidth * .5f; x++)
            {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < mapData.octaves; i++) {
					float sampleX = (x-halfWidth) / mapData.noiseScale * frequency + octaveOffsets[i].x;
					float sampleY = (y-halfHeight) / mapData.noiseScale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= mapData.persistance;
					frequency *= mapData.lacunarity;
				}

				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				noiseMap [new Vector2(x, y)] = noiseHeight;
			}
		}

		for (int y = (int)(mapData.mapHeight * -.5f); y < mapData.mapHeight * .5f; y++)
        {
            for (int x = (int)(mapData.mapWidth * -.5f); x < mapData.mapWidth * .5f; x++)
            {
				noiseMap [new Vector2(x, y)] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [new Vector2(x, y)]);
			}
		}

		return noiseMap;
	}

}
