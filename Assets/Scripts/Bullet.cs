using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
	[SerializeField]
	private float speed;

	private Rigidbody body;

	void Start () {
		body = GetComponent<Rigidbody> ();

		body.velocity = transform.rotation * (speed * Vector3.forward);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Zombie") {
			ZombieController zombie = collision.gameObject.GetComponent<ZombieController> ();
			if (zombie) {
				zombie.SendMessage("ReceiveDamage", 1, SendMessageOptions.DontRequireReceiver);
			}
		}

		Destroy (gameObject);
	}
}
