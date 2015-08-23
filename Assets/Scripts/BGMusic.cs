using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour {
	private static GameObject instance;

	void Awake () {
		if (instance == null) {
			instance = gameObject;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
