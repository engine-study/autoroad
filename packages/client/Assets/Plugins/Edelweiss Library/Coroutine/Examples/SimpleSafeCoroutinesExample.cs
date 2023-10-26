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
	
	public class SimpleSafeCoroutinesExample : MonoBehaviour {
		
		private SafeCoroutine m_SafeCoroutine1;
		private SafeCoroutine m_SafeCoroutine2;
		
		private void Awake () {
			m_SafeCoroutine1 = this.StartSafeCoroutine (ExampleCoroutine ());
			m_SafeCoroutine2 = this.StartSafeCoroutine (ExampleCoroutine ());
		}
		
		private IEnumerator ExampleCoroutine () {
			while (true) {
				yield return (null);
			}
		}
		
		private void OnGUI () {
			GUILayout.Label ("Coroutine 1: " + m_SafeCoroutine1.State);
			GUILayout.Label ("Coroutine 2: " + m_SafeCoroutine2.State);
			
			CoroutineGUI ("Coroutine 1", m_SafeCoroutine1);
			CoroutineGUI ("Coroutine 2", m_SafeCoroutine2);
		}
		
		private static void CoroutineGUI (string a_Name, SafeCoroutine a_SafeCoroutine) {
			GUILayout.BeginHorizontal ();
			GUI.enabled = a_SafeCoroutine.IsPaused;
			if (GUILayout.Button ("Resume " + a_Name)) {
				a_SafeCoroutine.Resume ();
			}
			GUI.enabled = a_SafeCoroutine.IsRunning;
			if (GUILayout.Button ("Pause " + a_Name)) {
				a_SafeCoroutine.Pause ();
			}
			GUI.enabled = a_SafeCoroutine.IsRunning || a_SafeCoroutine.IsPaused;
			if (GUILayout.Button ("Stop " + a_Name)) {
				a_SafeCoroutine.Stop ();
			}
			GUILayout.EndHorizontal ();
		}
	}
}