using UnityEngine;
using System.Collections;

public class UISplash : MonoBehaviour {
	void Awake () {
		Cursor.visible = false;
	}
	public void Go () {
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
