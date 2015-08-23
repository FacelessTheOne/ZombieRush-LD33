using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class CharController : MonoBehaviour {
	[SerializeField]
	protected float _speed;

	public virtual float speed {
		get { return _speed; }
	}

	[SerializeField]
	private int _hitPoints;
	public int hitPoints {
		get { return _hitPoints; }
	}
	
	protected Animator anim;
	protected Rigidbody body;
	
	private Vector3 _course;
	public Vector3 course {
		get { return _course; }
		set { _course = value.normalized; }
	}
	
	private float targetRotation;

	private bool _isAlive = true;
	public bool isAlive {
		get { return _isAlive; }
	}

	[SerializeField]
	protected GameObject bloodPrefab;
	
	// Use this for initialization
	protected virtual void Start () {
		anim = GetComponent<Animator> ();
		body = GetComponent<Rigidbody> ();
		
		Renderer rnd = GetComponent<Renderer> ();
		rnd.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
	}
	
	protected virtual void FixedUpdate () {
		if (isAlive) {
			body.velocity = new Vector3 (speed * course.x, body.velocity.y, speed * course.z);
			
			if (course.x != 0) {
				targetRotation = course.x > 0 ? 0f : -180f;
			}
		}
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (isAlive) {
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis (targetRotation, Vector3.up), 10f * Time.deltaTime);
			
			anim.SetBool ("IsWalking", course.sqrMagnitude > .1f);
		}
	}

	protected virtual void ReceiveDamage (int damage = 1) {
		if (isAlive) {
			_hitPoints -= damage;

			if (bloodPrefab != null) {
				Instantiate(bloodPrefab, transform.position + .5f * Vector3.up, Quaternion.identity);
			}

			if (hitPoints <= 0) {
				Kill ();
			}
		}
	}

	protected virtual void Kill () {
		if (isAlive) {
			_isAlive = false;
			StopAllCoroutines();
			body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			anim.SetBool("IsAlive", false);
		}
	}
}
