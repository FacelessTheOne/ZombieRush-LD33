using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform target;

	public Vector3 offset;

	public float smoothTime;

	private Vector3 cameraVelocity = Vector3.zero;
	// Update is called once per frame

	[ExecuteInEditMode]
	void Start () {
		if (target != null) {
			transform.position = GetPosition();
		}
	}

	void FixedUpdate () {
		if (target != null) {
			transform.position = Vector3.SmoothDamp (transform.position, (GetPosition()), ref cameraVelocity, smoothTime);
		}
	}

	public Vector3 GetPosition () {
		if (target != null) {
			return target.position + offset;
		}

		return Vector3.zero;
	}
}
