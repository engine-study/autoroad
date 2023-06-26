using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

	public Renderer textureRender;
	public Material material;
	void Awake() {
		
	}

	public void DrawTexture(Texture2D texture) {

		if(material != null) {
			DestroyImmediate(material);
		}

		material = new Material(Shader.Find("Unlit/Texture"));
		material.mainTexture = texture;

		textureRender.sharedMaterial = material;
		textureRender.transform.localScale = new Vector3 (texture.width * .1f, 1, texture.height * .1f);
	
		Debug.Log("Map updated.", this);
	}
	
}
