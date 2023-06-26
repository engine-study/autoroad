using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		MapGenerator mapGen = (MapGenerator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				mapGen.Create ();
			}
		}

		if(!mapGen.gameObject.scene.name.Contains("NonSerialized")) {
			Debug.LogError("Wait, don't use this in serialized scenes, you'll clog up the github :(");
			return;
		}

		if (GUILayout.Button ("Create")) {
			mapGen.Create();
		}

		if (GUILayout.Button ("Fog")) {
			mapGen.ToggleFog();
		}
	}

	void OnValidate() {

		// Debug.Log("Editor validate");
	}
}
