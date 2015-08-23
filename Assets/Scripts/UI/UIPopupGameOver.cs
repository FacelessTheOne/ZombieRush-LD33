using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class UIPopupGameOver : UIPopup {

	[SerializeField]
	private Text killedText;

	public void Retry () {
		GameController.instance.Reload ();
	}

	public void Exit () {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}

	public override void Show () {
		base.Show ();
		StartCoroutine (ShowCoroutine());
	}

	private IEnumerator ShowCoroutine () {
		killedText.text = "0";

		yield return StartCoroutine(CoroutineUtils.WaitForSecondsUnscaled (1f));

		int killed = GameController.instance.peopleKilled;
		for (int i = 1; i <= killed; i++) {
			killedText.text = i.ToString();
			yield return CoroutineUtils.WaitForSecondsUnscaled(0.01f);
		}
	}
}
