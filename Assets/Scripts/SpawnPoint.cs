using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {
	private const float MIN_SPAWN_DELAY = 1f;

	[SerializeField]
	private GameObject spawn;

	[SerializeField]
	private float spawnDelay;

	[SerializeField]
	private float spawnDelayReduceInterval;

	[SerializeField]
	private float spawnDelayReduceMultiplier;

	private float lastSpawnTime;

	private float lastRateIncreaseTime;

	void Start () {
		lastSpawnTime = Time.time;
		lastRateIncreaseTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (!GameController.instance.isGameOver) {
			if (Time.time - lastSpawnTime > spawnDelay) {
				Spawn ();
				lastSpawnTime = Time.time;
			}

			if (Time.time - lastRateIncreaseTime > spawnDelayReduceInterval) {
				spawnDelay *= spawnDelayReduceMultiplier;
				spawnDelay = Mathf.Clamp (spawnDelay, MIN_SPAWN_DELAY, float.MaxValue);
				lastRateIncreaseTime = Time.time;
			}
		}
	}

	private void Spawn () {
		if (spawn != null) {
			Instantiate (spawn, transform.position, Quaternion.identity);
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, .5f);
	}
}
