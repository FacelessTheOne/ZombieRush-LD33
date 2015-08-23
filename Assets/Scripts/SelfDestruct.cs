using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	[SerializeField]
	private float delay;

	void Start () {
		Destroy (gameObject, delay);
	}
}
