using UnityEngine;
using System.Collections;

public class HumanController : CharController {
	private static float TURN_TO_UNDEAD_DELAY = 3f;
	private static float UPDATE_STATE_INTERVAL = 1f;

	[SerializeField]
	private float panicSpeed;

	public override float speed {
		get {
			if (isPanic) {
				return panicSpeed;
			} else {
				return base.speed;
			}
		}
	}

	private bool isPanic = false;

	[SerializeField]
	private GameObject zombiePrefab;

	[SerializeField]
	private SearchTrigger zombieSearchTrigger;

	[SerializeField]
	private Transform soundHitContainer;
	private AudioSource[] soundHit;

	protected override void Start ()
	{
		base.Start ();

		soundHit = soundHitContainer.GetComponents<AudioSource> ();

		StartCoroutine (UpdateStateCoroutine());
	}

	private IEnumerator UpdateStateCoroutine () {
		while (isAlive) {
			UpdateState();
			yield return new WaitForSeconds(UPDATE_STATE_INTERVAL);
		}
	}

	private void UpdateState () {
		if (isAlive) {
			ZombieController[] zombies = zombieSearchTrigger.GetFoundComponents<ZombieController> ();

			isPanic = zombies.Length > 0;

			course = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "ZombieHit") {
			ZombieController zombie = other.GetComponentInParent<ZombieController>();
			if (zombie != null && zombie.isAlive) {
				ReceiveDamage(1);
			}
		}
	}

	protected override void Kill () {
		bool wereAlive = isAlive;
		base.Kill ();

		if (wereAlive) {
			StartCoroutine(TurnToUndeadCoroutine());
			GameController.instance.SendMessage("OnKill");
		}
	}

	private IEnumerator TurnToUndeadCoroutine () {
		yield return new WaitForSeconds (TURN_TO_UNDEAD_DELAY);

		if (!GameController.instance.isGameOver && zombiePrefab != null) {
			Instantiate (zombiePrefab, new Vector3 (transform.position.x, 0f, transform.position.z), Quaternion.identity);
		}
		if (bloodPrefab != null) {
			Instantiate(bloodPrefab, transform.position + .5f * Vector3.up, Quaternion.identity);
		}
		Destroy (gameObject);
	}

	protected override void ReceiveDamage (int damage)
	{
		AudioSource hit = soundHit [Random.Range (0, soundHit.Length)];
		hit.Stop ();
		hit.Play ();
		base.ReceiveDamage (damage);
	}
}
