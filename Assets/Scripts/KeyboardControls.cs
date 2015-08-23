using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ZombieController))]
public class KeyboardControls : MonoBehaviour {

	private ZombieController zombie;
	// Use this for initialization
	void Start () {
		zombie = GetComponent<ZombieController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 course = new Vector3(Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));

		zombie.course = course;
	}
}
