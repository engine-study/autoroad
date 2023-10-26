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
	
	public class NestedSafeCoroutinesExample : MonoBehaviour {
		
		[SerializeField] private bool m_FinishOuterCoroutineAutomatically = false;
		
		private SafeCoroutine m_OuterCoroutine;
		private SafeCoroutine m_InnerCoroutine;
		private float m_InnerCoroutineTimer = 0.0f;
		
		private void Awake () {
			m_OuterCoroutine = this.StartSafeCoroutine (OuterExampleCoroutine ());
		}
		
		private IEnumerator OuterExampleCoroutine () {
			m_InnerCoroutine = this.StartSafeCoroutine (InnerExampleCoroutine ());
			yield return (m_InnerCoroutine);
			
				// Continue after the inner coroutine was finished.
			while (!m_FinishOuterCoroutineAutomatically) {
				yield return (null);
			}
		}
		
		private IEnumerator InnerExampleCoroutine () {
			while (true) {
				m_InnerCoroutineTimer = m_InnerCoroutineTimer + Time.deltaTime;
				yield return (null);
			}
		}
		
		private void OnGUI () {
			CoroutineStateGUI ("Outer Coroutine", m_OuterCoroutine);
			CoroutineStateGUI ("Inner Coroutine", m_InnerCoroutine);
			GUILayout.Label ("Inner Coroutine Timer: " + m_InnerCoroutineTimer.ToString ("0.0"));
			
			CoroutineGUI ("Outer Coroutine", m_OuterCoroutine);
			CoroutineGUI ("Inner Coroutine", m_InnerCoroutine);
		}
		
		private static void CoroutineStateGUI (string a_Name, SafeCoroutine a_SafeCoroutine) {
			string l_CoroutineStateText = a_Name + ": " + a_SafeCoroutine.State;
			if (a_SafeCoroutine.IsSelfPaused) {
				l_CoroutineStateText = l_CoroutineStateText + "; Self paused";
			}
			if (a_SafeCoroutine.IsParentPaused) {
				l_CoroutineStateText = l_CoroutineStateText + "; Parent paused";
			}
			GUILayout.Label (l_CoroutineStateText);
		}
		
		private static void CoroutineGUI (string a_Name, SafeCoroutine a_SafeCoroutine) {
			GUILayout.BeginHorizontal ();
			GUI.enabled = a_SafeCoroutine.IsSelfPaused;
			if (GUILayout.Button ("Resume " + a_Name)) {
				a_SafeCoroutine.Resume ();
			}			
			GUI.enabled = a_SafeCoroutine.IsRunning || (a_SafeCoroutine.IsParentPaused && !a_SafeCoroutine.IsSelfPaused);
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