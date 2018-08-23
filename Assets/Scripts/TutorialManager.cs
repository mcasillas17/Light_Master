using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

	public GameObject currentCheckPoint;
	public LightMasterController player;
	public Camera mainCam;

	public GameObject deathParticle;
	public GameObject respawnParticle;

	public float gravityStore;
	public float respawnDelay;

	// Use this for initialization
	void Awake () {
		player = FindObjectOfType<LightMasterController> ();
		currentCheckPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		respawnDelay = 1.5f;
		gravityStore = player.GetComponent<Rigidbody2D> ().gravityScale;
		mainCam = Camera.main;
	}

	public void respawnPlayer(){
		StartCoroutine ("respawnPlayerCorutine");
	}

	public IEnumerator respawnPlayerCorutine(){

		Instantiate (deathParticle, player.transform.position, Quaternion.identity);
		player.gameObject.SetActive (false);
		player.GetComponent<Renderer> ().enabled = false;
		player.GetComponent<Rigidbody2D> ().gravityScale = 0f;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		yield return new WaitForSeconds (respawnDelay);

		if (!player.isDead ()) {

			player.GetComponent<Rigidbody2D> ().gravityScale = gravityStore;
			player.transform.position = new Vector3 (currentCheckPoint.transform.position.x, currentCheckPoint.transform.position.y, player.transform.position.z);
			mainCam.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, mainCam.transform.position.z);
			Instantiate (respawnParticle, player.transform.position, Quaternion.identity);
			player.gameObject.SetActive (true);
			player.GetComponent<Renderer> ().enabled = true;

		} else {
			string currentScene = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene (currentScene);

		}
	}

	public void MoveToNextLevel(){
		SceneManager.LoadScene ("DirtLevel");
	}
}
