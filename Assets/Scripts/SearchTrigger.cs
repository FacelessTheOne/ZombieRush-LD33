using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class SearchTrigger : MonoBehaviour {
	[SerializeField]
	private string tagToSearch;

	private HashSet<GameObject> set = new HashSet<GameObject>();

	void OnTriggerEnter (Collider other) {
		if (other.tag == tagToSearch) {
			set.Add(other.gameObject.transform.parent.gameObject);
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == tagToSearch) {
			set.Remove(other.gameObject.transform.parent.gameObject);
		}
	}

	public T[] GetFoundComponents<T> () where T : MonoBehaviour {
		List<T> result = new List<T> (set.Count);

		foreach (GameObject obj in set) {
			if (obj == null) continue;

			T component = obj.GetComponent<T>();
			if (component != null) {
				result.Add(component);
			}
		}

		return result.ToArray();
	}
}
