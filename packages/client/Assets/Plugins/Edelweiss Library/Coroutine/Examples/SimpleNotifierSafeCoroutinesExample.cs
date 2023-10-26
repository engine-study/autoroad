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
	
	public class SimpleNotifierSafeCoroutinesExample : MonoBehaviour {
		
		private void Awake () {
			SafeCoroutine <float> l_SafeCoroutine = this.StartSafeCoroutine <float> (ResultReturningCoroutine ());
			l_SafeCoroutine.GenericStateChangeNotifier.Subscribe (OnResultReturningSafeCoroutineStateChange);
		}
		
		private IEnumerator ResultReturningCoroutine () {
			yield return (new WaitForSeconds (1.0f));
			yield return (Mathf.PI);
		}
		
		private void OnResultReturningSafeCoroutineStateChange (SafeCoroutine <float> a_SafeCoroutine, SafeCoroutineState a_State) {
			Debug.Log ("Result Returning Safe Coroutine " + a_State + "; " + Time.realtimeSinceStartup);
			if (a_SafeCoroutine.HasResult) {
				Debug.Log ("Result: " + a_SafeCoroutine.Result);
			}
		}
	}
}