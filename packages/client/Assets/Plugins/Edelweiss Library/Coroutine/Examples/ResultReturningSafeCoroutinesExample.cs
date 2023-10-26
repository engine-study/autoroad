//
// Author:
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2014 Edelweiss Interactive (http://www.edelweissinteractive.com)
//

using UnityEngine;
using System;
using System.Collections;
using Edelweiss.Coroutine;

namespace Edelweiss.Coroutine.Examples {
	
	public class ResultReturningSafeCoroutinesExample : MonoBehaviour {
		
		private SafeCoroutine <float> m_ResultReturningCoroutine;
		private bool m_ReturnResultInNextFrame = false;
		
		private void Awake () {
			m_ResultReturningCoroutine = this.StartSafeCoroutine <float> (ResultReturningCoroutine ());
		}
		
		private IEnumerator ResultReturningCoroutine () {
			while (true) {
				if (m_ReturnResultInNextFrame) {
					yield return (Mathf.PI);
					break;
				}
				yield return (null);
			}
		}
		
		private void OnGUI () {
			GUILayout.Label ("Result Returning Coroutine: " + m_ResultReturningCoroutine.State);
			if (m_ResultReturningCoroutine.HasResult) {
				GUILayout.Label (m_ResultReturningCoroutine.Result.ToString ());
			}
			
			GUI.enabled = m_ResultReturningCoroutine.IsRunning;
			if (GUILayout.Button ("Return Value")) {
				m_ReturnResultInNextFrame = true;
			}
		}
	}
}