using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FalloffGenerator : MonoBehaviour {

	public float weight = 1f;
	public bool invert;
	public float falloff = 3;
	public float start = 4f;

	public Dictionary<Vector2, float> GenerateFalloffMap(Vector3 position, int width, int height) {
		Dictionary<Vector2, float> map = new Dictionary<Vector2, float>();

		for (int j = (int)(position.z + height * -.5f); j < position.z + height * .5f; j++)
        {
            for (int i = (int)(position.x + width * -.5f); i < position.x + width * .5f; i++)
            {
				// float x = i / (float)width * 2 - 1;
				// float y = j / (float)height * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (i), Mathf.Abs (j));

				value = Evaluate(value, falloff, start);

				if(!invert) {
					value = 1f - value;
				}

				map [new Vector2(i, j)] = value;
				
			}
		}

		return map;
	}

	static float Evaluate(float value, float falloff, float start) {
		return Mathf.Pow (value, falloff) / (Mathf.Pow (value, falloff) + Mathf.Pow (start - start * value, falloff));
	}
}
