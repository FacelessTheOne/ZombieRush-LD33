using UnityEngine;
using System.Collections;

public class UIPopupPause : UIPopup {
	public void Resume () {
		GameController.instance.Resume ();
	}

	public void Exit () {
		GameController.instance.Exit ();
	}
}
