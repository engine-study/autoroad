//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2014 Edelweiss Interactive (http://www.edelweissinteractive.com)
//

using UnityEngine;
using System.Collections;
using Edelweiss.Coroutine;

namespace Edelweiss.Coroutine.Examples {
	
	public class ExecutionOrderSafeCoroutinesExample : MonoBehaviour {
		
		private void Start () {
			StartCoroutine (FixedUpdateCoroutine ("Unity Coroutine Pre"));
			StartCoroutine (UpdateCoroutine ("Unity Coroutine Pre"));
			StartCoroutine (EndOfFrameCoroutine ("Unity Coroutine Pre"));
			
			this.StartSafeCoroutine (FixedUpdateCoroutine ("Safe Coroutine"));
			this.StartSafeCoroutine (UpdateCoroutine ("Safe Coroutine"));
			this.StartSafeCoroutine (EndOfFrameCoroutine ("Safe Coroutine"));
			
			StartCoroutine (FixedUpdateCoroutine ("Unity Coroutine Post"));
			StartCoroutine (UpdateCoroutine ("Unity Coroutine Post"));
			StartCoroutine (EndOfFrameCoroutine ("Unity Coroutine Post"));
		}
		
		private IEnumerator FixedUpdateCoroutine (string a_Name) {
			while (true) {
				yield return (new WaitForFixedUpdate ());
				Debug.Log ("Fixed Update " + a_Name);
			}
		}
		
		private IEnumerator UpdateCoroutine (string a_Name) {
			while (true) {
				yield return (null);
				Debug.Log ("Update " + a_Name);
			}
		}
		
		private IEnumerator EndOfFrameCoroutine (string a_Name) {
			while (true) {
				yield return (new WaitForEndOfFrame ());
				Debug.Log ("End Of Frame " + a_Name);
			}
		}
		
		private void FixedUpdate () {
			Debug.Log ("Fixed Update");
		}
		
		private void Update () {
			Debug.Log ("Update");
		}
		
		private void LateUpdate () {
			Debug.Log ("Late Update");
		}
	}
}