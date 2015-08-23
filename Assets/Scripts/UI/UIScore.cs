using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIScore : MonoBehaviour {

	[SerializeField]
	private Text kills;

	[SerializeField]
	private Text horde;

	// Update is called once per frame
	void Update () {
		kills.text = GameController.instance.peopleKilled.ToString ();

		horde.text = GameController.instance.horde.ToString ();
	}
}
