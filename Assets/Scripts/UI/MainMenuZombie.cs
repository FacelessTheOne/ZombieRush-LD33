using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MainMenuZombie : MonoBehaviour {
	[SerializeField]
	private float speed;

	[SerializeField]
	private float changeInterval;

	private Rigidbody body;

	private Vector3 targetVelocity = Vector3.zero;

	void Start () {
		body = GetComponent<Rigidbody> ();

		StartCoroutine (ChangeDirectionCoroutine());
	}

	void Update () {
		if (Mathf.Abs (body.velocity.x) > 0.1f) {
			transform.rotation = Quaternion.AngleAxis(body.velocity.x > 0f ? 0f : 180f, Vector3.up);
		}
	}

	void FixedUpdate () {
		body.velocity = targetVelocity;
	}

	private IEnumerator ChangeDirectionCoroutine () {
		while (true) {
			targetVelocity = new Vector3 (Random.Range (-1f, 1f) * speed, 0f, 0f);
			yield return new WaitForSeconds (changeInterval);
		}
	}
}
