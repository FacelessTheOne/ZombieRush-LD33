using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class ZombieController : CharController {
	[SerializeField]
	private bool _isLeader = false;

	public bool isLeader {
		get { return _isLeader; }
	}

	[SerializeField]
	private Renderer selection;

	[SerializeField]
	private GameObject soundGrowlContainer;
	private AudioSource[] soundGrowl;

	protected override void Start () {
		base.Start ();

		soundGrowl = soundGrowlContainer.GetComponents<AudioSource> ();

		if (isAlive) {
			Growl();
			GameController.instance.SendMessage("OnZombieBirth");
		}
	}

	protected override void Update () {
		base.Update ();

		if (selection != null) {
			selection.enabled = isLeader && isAlive;
		}
	}

	private void Growl () {
		AudioSource growl = soundGrowl [Random.Range (0, soundGrowl.Length)];
		growl.Stop ();
		growl.Play ();
	}

	protected override void Kill () {
		if (isAlive && isLeader) {
			ZombieController[] zombies = FindObjectsOfType<ZombieController>();

			ZombieController newLeader = null;
			float minDistanceSqr = float.MaxValue;

			foreach (ZombieController zombie in zombies) {
				if (zombie.gameObject == gameObject) continue;
				if (!zombie.isAlive) continue;

				float distanceSqr = (zombie.transform.position - transform.position).sqrMagnitude;

				if (distanceSqr < minDistanceSqr) {
					newLeader = zombie;
					minDistanceSqr = distanceSqr;
				}
			}

			if (newLeader != null) {
				CameraController cam = Camera.main.GetComponent<CameraController>();
				cam.target = newLeader.transform;

				ZombieFollow follow = newLeader.GetComponent<ZombieFollow>();
				if (follow != null) {
					Destroy(follow);
				}

				KeyboardControls keyCtrls = newLeader.GetComponent<KeyboardControls>();
				if (keyCtrls == null) {
					newLeader.gameObject.AddComponent<KeyboardControls>();
				} else {
					keyCtrls.enabled = true;
				}

				newLeader._isLeader = true;
			}
		}
		_isLeader = false;

		if (isAlive) {
			GameController.instance.SendMessage("OnZombieDeath");
		}

		base.Kill ();

		StartCoroutine (FinalDestroyCoroutine());
	}

	private IEnumerator FinalDestroyCoroutine () {
		yield return new WaitForSeconds(5f);

		Instantiate (bloodPrefab, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	protected override void ReceiveDamage (int damage) {
		base.ReceiveDamage (damage);
		Growl();
	}
}
