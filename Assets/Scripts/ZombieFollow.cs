using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ZombieController))]
public class ZombieFollow : MonoBehaviour {
	private static float UPDATE_FOLLOW_TARGET_INTERVAL = .5f;

	[SerializeField]
	private float radius;
	private float radiusSqr;

	[SerializeField]
	private SearchTrigger humanSearch;
	
	[SerializeField]
	private SearchTrigger zombieSearch;

	private ZombieController zombie;

	private CharController target;

	void Start () {
		radiusSqr = radius * radius;
		zombie = GetComponent<ZombieController> ();

		StartCoroutine (UpdateFollowTargetCoroutine());
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector3 course = Vector3.zero;
		if (target != null) {
			Vector3 distanceVector = target.transform.position - transform.position;


			if (distanceVector.sqrMagnitude > radiusSqr || !(target is ZombieController)) {
				 course = distanceVector;
			}
		}
		zombie.course = course;
	}

	public bool HasTarget () {
		return target != null;
	}

	private void UpdateFollowTarget () {
		if (target != null) {
			if (!target.isAlive) {
				target = null;
			}
		}

		CharController[] humans = humanSearch.GetFoundComponents<CharController> ();

		CharController newTarget = null;
		foreach (CharController human in humans) {
			if (human == null) continue;
			if (human.gameObject == gameObject) continue;
			if (!human.isAlive) continue;

			newTarget = human;
		}

		if (newTarget != null) {
			target = newTarget;
			return;
		}

		ZombieController[] zombies = zombieSearch.GetFoundComponents<ZombieController> ();
		float minDistanceSqr = float.MaxValue;
		foreach (ZombieController cZombie in zombies) {
			if (cZombie == null) continue;
			if (cZombie.gameObject == gameObject) continue;
			if (!cZombie.isAlive) continue;
			
			float distanceSqr = (transform.position - cZombie.transform.position).sqrMagnitude;
			
			if (cZombie.isLeader) {
				newTarget = cZombie;
				break;
			}
			
			if (distanceSqr < minDistanceSqr) {
				newTarget = cZombie;
				minDistanceSqr = distanceSqr;
			}
		}

		if (newTarget != null) {
			target = newTarget;
			return;
		}

		/*

		CharController[] chars = FindObjectsOfType<CharController> ();
		
		CharController newTarget = null;
		float minDistanceSqr = float.MaxValue;
		
		foreach (CharController chr in chars) {
			if (chr.gameObject == gameObject) continue;
			if (!chr.isAlive) continue;
			
			float distanceSqr = (transform.position - chr.transform.position).sqrMagnitude;

			if (distanceSqr > searchRadiusSqr) continue;

			if (chr.tag == "Human") {
				newTarget = chr;
				break;
			}

			ZombieController cZombie = chr.GetComponent<ZombieController>();

			if (cZombie != null) {
				if (cZombie.isLeader) {
					newTarget = cZombie;
					break;
				}
				
				if (distanceSqr < minDistanceSqr) {
					newTarget = cZombie;
					minDistanceSqr = distanceSqr;
				}
			}
		}

		if (newTarget != null) {
			target = newTarget;
		}
		 */
	}

	private IEnumerator UpdateFollowTargetCoroutine () {
		while (zombie.isAlive) {
			UpdateFollowTarget();
			yield return new WaitForSeconds(UPDATE_FOLLOW_TARGET_INTERVAL);
		}
	}
}
