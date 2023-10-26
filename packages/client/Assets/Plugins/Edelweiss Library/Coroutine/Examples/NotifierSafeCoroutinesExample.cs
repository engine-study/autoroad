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
	
	public class NotifierSafeCoroutinesExample : MonoBehaviour {
		
		private SafeCoroutine m_SafeCoroutine;
		private bool m_FinishSafeCoroutineInNextFrame = false;
		
		private SafeCoroutine <float> m_ResultReturningSafeCoroutine;
		private bool m_FinishResultReturningSafeCoroutineInNextFrame = false;
		
		private Vector2 m_ScrollPosition;
		private string m_Messages = string.Empty;
		
		private void Awake () {
			m_SafeCoroutine = this.StartSafeCoroutine (SimpleCoroutine ());
			m_SafeCoroutine.StateChangeNotifier.Subscribe (OnSafeCoroutineStateChange);
			
			m_ResultReturningSafeCoroutine = this.StartSafeCoroutine <float> (ResultReturningCoroutine ());
			m_ResultReturningSafeCoroutine.GenericStateChangeNotifier.Subscribe (OnResultReturningSafeCoroutineStateChange);
		}
		
		private IEnumerator SimpleCoroutine () {
			while (true) {
				if (m_FinishSafeCoroutineInNextFrame) {
					break;
				}
				yield return (null);
			}
		}
		
		private IEnumerator ResultReturningCoroutine () {
			while (true) {
				if (m_FinishResultReturningSafeCoroutineInNextFrame) {
					yield return (Mathf.PI);
					break;
				}
				yield return (null);
			}
		}
		
		private void OnSafeCoroutineStateChange (SafeCoroutineState a_State) {
			m_Messages = m_Messages + "Safe Coroutine " + a_State + "; " + Time.realtimeSinceStartup + "\n";
		}
		
		private void OnResultReturningSafeCoroutineStateChange (SafeCoroutine <float> a_SafeCoroutine, SafeCoroutineState a_State) {
			m_Messages = m_Messages + "Result Returning Safe Coroutine " + a_State + "; " + Time.realtimeSinceStartup + "\n";
			if (a_SafeCoroutine.HasResult) {
				m_Messages = m_Messages + "Result: " + a_SafeCoroutine.Result + "\n";
			}
		}
		
		private void OnGUI () {
			GUILayout.Label ("Safe Coroutine: " + m_SafeCoroutine.State);
			GUILayout.Label ("Result Returning Safe Coroutine: " + m_ResultReturningSafeCoroutine.State);
			
			GUILayout.BeginHorizontal ();
			GUI.enabled = m_SafeCoroutine.IsPaused;
			if (GUILayout.Button ("Resume")) {
				m_SafeCoroutine.Resume ();
			}
			GUI.enabled = m_SafeCoroutine.IsRunning;
			if (GUILayout.Button ("Pause")) {
				m_SafeCoroutine.Pause ();
			}
			GUI.enabled = m_SafeCoroutine.IsRunning || m_SafeCoroutine.IsPaused;
			if (GUILayout.Button ("Stop")) {
				m_SafeCoroutine.Stop ();
			}
			GUI.enabled = m_SafeCoroutine.IsRunning;
			if (GUILayout.Button ("Finish")) {
				m_FinishSafeCoroutineInNextFrame = true;
			}
			GUILayout.EndHorizontal ();
			
			GUILayout.BeginHorizontal ();
			GUI.enabled = m_ResultReturningSafeCoroutine.IsPaused;
			if (GUILayout.Button ("Resume")) {
				m_ResultReturningSafeCoroutine.Resume ();
			}
			GUI.enabled = m_ResultReturningSafeCoroutine.IsRunning;
			if (GUILayout.Button ("Pause")) {
				m_ResultReturningSafeCoroutine.Pause ();
			}
			GUI.enabled = m_ResultReturningSafeCoroutine.IsRunning || m_ResultReturningSafeCoroutine.IsPaused;
			if (GUILayout.Button ("Stop")) {
				m_ResultReturningSafeCoroutine.Stop ();
			}
			GUI.enabled = m_ResultReturningSafeCoroutine.IsRunning;
			if (GUILayout.Button ("Finish")) {
				m_FinishResultReturningSafeCoroutineInNextFrame = true;
			}
			GUILayout.EndHorizontal ();
			
			GUI.enabled = true;
			m_ScrollPosition = GUILayout.BeginScrollView (m_ScrollPosition);
			GUILayout.Label (m_Messages);
			GUILayout.EndScrollView ();
		}
	}
}