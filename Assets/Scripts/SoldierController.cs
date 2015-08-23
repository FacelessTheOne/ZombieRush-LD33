using UnityEngine;
using System.Collections;

public class SoldierController : CharController {
	private static float TURN_TO_UNDEAD_DELAY = 3f;
	private static float UPDATE_STATE_INTERVAL = .5f;
	private float SHOOT_DURATION;

	[SerializeField]
	private GameObject zombiePrefab;

	[SerializeField]
	private SearchTrigger zombieSearch;

	[SerializeField]
	private float fireRate;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private Transform firePoint;

	[SerializeField]
	private Transform soundShotContainer;
	private AudioSource[] soundShot;

	[SerializeField]
	private Transform soundHitContainer;
	private AudioSource[] soundHit;

	private Coroutine shootingCoroutine = null;

	private Transform target;
	
	protected override void Start ()
	{
		base.Start ();

		SHOOT_DURATION = 1f / fireRate;

		soundShot = soundShotContainer.GetComponents<AudioSource> ();

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
			Vector3 newCourse = Vector3.zero;
			ZombieController[] zombies = zombieSearch.GetFoundComponents<ZombieController> ();

			target = null;

			foreach (ZombieController zombie in zombies) {
				if (!zombie.isAlive) continue;

				newCourse = zombie.transform.position - transform.position;
				target = zombie.transform;
				break;
			}

			if (newCourse == Vector3.zero) {
				course = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			} else {
				course = newCourse;
			}
		}
	}

	protected override void FixedUpdate () {
		base.FixedUpdate ();

		if (isAlive && shootingCoroutine != null) {
			body.velocity = Vector3.zero;
		}
	}

	protected override void Update () {
		base.Update ();

		if (target != null && isAlive) {
			Shoot(target.position);
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

	private void Shoot (Vector3 target) {
		if (shootingCoroutine == null) {
			shootingCoroutine = StartCoroutine (ShootCoroutine (target));
		}
	}

	private IEnumerator ShootCoroutine (Vector3 target) {
		anim.SetBool ("IsWalking", false);
		anim.SetTrigger ("Shoot");

		Vector3 lookVector = target - firePoint.position;
		lookVector = new Vector3 (lookVector.x, 0f, lookVector.z);

		Quaternion rotation = Quaternion.LookRotation(lookVector, Vector3.up);

		Instantiate (bulletPrefab, firePoint.position, rotation);

		AudioSource shot = soundShot [Random.Range (0, soundShot.Length)];
		shot.Stop ();
		shot.Play ();

		yield return new WaitForSeconds (SHOOT_DURATION);
		shootingCoroutine = null;
	}

	protected override void ReceiveDamage (int damage)
	{
		AudioSource hit = soundHit [Random.Range (0, soundHit.Length)];
		hit.Stop ();
		hit.Play ();

		base.ReceiveDamage (damage);
	}
}
