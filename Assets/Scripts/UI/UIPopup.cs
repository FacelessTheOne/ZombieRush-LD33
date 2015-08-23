using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UIPopup : MonoBehaviour {

	private Animator anim;

	[SerializeField]
	private EventSystem eventSystem;

	[SerializeField]
	private GameObject firstSelected;

	protected virtual void Start () {
		anim = GetComponent<Animator> ();
	}

	public virtual void Show () {
		anim.SetBool ("IsShown", true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		if (firstSelected != null && eventSystem != null) {
			eventSystem.SetSelectedGameObject(firstSelected, new BaseEventData(eventSystem));
		}
	}

	public virtual void Hide () {
		anim.SetBool ("IsShown", false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
