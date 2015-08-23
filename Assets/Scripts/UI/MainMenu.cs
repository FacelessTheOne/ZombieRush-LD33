using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class MainMenu : MonoBehaviour {

	void Awake () {
		Cursor.visible = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Return)) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {

#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#else
			Application.Quit ();
#endif
		}
	}
}
