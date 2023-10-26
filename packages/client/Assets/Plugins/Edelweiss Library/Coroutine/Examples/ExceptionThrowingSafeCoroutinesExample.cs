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

	public class ExceptionThrowingSafeCoroutinesExample : MonoBehaviour {
	
		private SafeCoroutine m_ExceptionThrowingCoroutine;
		
		private bool m_ThrowExceptionInNextFrame = false;
		
		private void Awake () {
			m_ExceptionThrowingCoroutine = this.StartSafeCoroutine (ExceptionThrowingCoroutine ());
			m_ExceptionThrowingCoroutine.CatchExceptions = true;
		}
	
		private IEnumerator ExceptionThrowingCoroutine () {
			while (true) {
				if (m_ThrowExceptionInNextFrame) {
					throw (new InvalidOperationException ("You are not allowed to click that button!"));
				}
				yield return (null);
			}
		}
		
		private void OnGUI () {
			GUILayout.Label ("Exception Throwing Coroutine: " + m_ExceptionThrowingCoroutine.State);
			if (m_ExceptionThrowingCoroutine.ThrewException) {
				GUILayout.Label (m_ExceptionThrowingCoroutine.ThrownException.GetType ().Name);
				GUILayout.Label (m_ExceptionThrowingCoroutine.ThrownException.Message);
			}
						
			GUI.enabled = m_ExceptionThrowingCoroutine.IsRunning;
			if (GUILayout.Button ("Throw Exception")) {
				m_ThrowExceptionInNextFrame = true;
			}
		}
	}
}