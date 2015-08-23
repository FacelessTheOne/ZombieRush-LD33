using UnityEngine;
using System.Collections;

public static class CoroutineUtils {
	public static IEnumerator WaitForSecondsUnscaled (float seconds) {
		float start = Time.realtimeSinceStartup;

		while (Time.realtimeSinceStartup < start + seconds) {
			yield return null;
		}
	}
}
