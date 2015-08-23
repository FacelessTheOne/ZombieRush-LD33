using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class GameController : MonoBehaviour {

	private static GameController _instance;
	public static GameController instance {
		get { return _instance; }
	}

	private int _peopleKilled = 0;
	public int peopleKilled {
		get { return _peopleKilled; }
	}

	private int _horde = 0;
	public int horde {
		get { return _horde; }
	}

	private bool _isGameOver = false;
	public bool isGameOver {
		get { return _isGameOver; }
	}

	[SerializeField]
	private UIPopup gameOverPopup;

	[SerializeField]
	private UIPopup pausePopup;

	void Awake () {
		if (_instance == null) {
			_instance = this;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Destroy(this);
		}
	}

	void OnKill () {
		_peopleKilled++;
	}

	void OnZombieBirth () {
		_horde++;
	}

	void OnZombieDeath () {
		_horde--;

		if (_horde == 0) {
			GameOver();
		}
	}

	private void GameOver () {
		_isGameOver = true;
		StartCoroutine (GameOverCoroutine());
	}

	private IEnumerator GameOverCoroutine () {
		yield return new WaitForSeconds (2f);
		Time.timeScale = 0f;

		if (gameOverPopup != null) {
			gameOverPopup.Show();
		}
	}

	public void Reload () {
		Time.timeScale = 1f;
		Application.LoadLevel (Application.loadedLevel);
	}

	public void Exit () {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}

	public void Resume () {
		if (!_isGameOver) {
			Time.timeScale = 1f;

			pausePopup.Hide();
		}
	}

	public void Pause () {
		if (!_isGameOver) {
			Time.timeScale = 0f;

			pausePopup.Show();
		}
	}

	public void PauseToggle () {
		if (!_isGameOver) {
			if (Time.timeScale > 0f) {
				Pause();
			} else {
				Resume();
			}
		}
	}

	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			PauseToggle();
		}
	}
}
