using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : MonoBehaviour {

	public Transform firePoint;
	public GameObject arrow;
	public int damageToPlayer;
	public int coinPenalty;
	public LevelManager levelManager;
	public int lightsOnLauncher;
	public int launcherType;

	float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
	float[] hVelocities = { 0f, -4.0f, -4.0f, -4.0f, 0f, 4.0f, 4.0f, 4.0f };
	float[] vVelocities = {4.0f, 4.0f, 0f, -4.0f, -4.0f, -4.0f, 0f, 4.0f };

	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		StartCoroutine (fireArrows ());
		lightsOnLauncher = 0;
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Light") {
			lightsOnLauncher++;
		}
		if (other.gameObject.tag == "Player") {
			levelManager.respawnPlayer ();
			other.gameObject.GetComponent<LightMasterController> ().setDamage (damageToPlayer);
			CoinsManager.addCoins (-coinPenalty);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Light") {
			lightsOnLauncher--;
		}
	}

	IEnumerator fireArrows(){
		while (true) {
			if (lightsOnLauncher==0) {
				if (launcherType == 0) {
					for (int i = 0; i < angles.Length; i++) {
						GameObject tempArrow = (GameObject)(Instantiate (arrow, firePoint.position, Quaternion.Euler (new Vector3 (0, 0, angles [i] + 90))));
						ArrowController arrowCont = tempArrow.GetComponent<ArrowController> ();
						arrowCont.hVelocity = hVelocities [i];
						arrowCont.vVelocity = vVelocities [i];
					}
				} else if (launcherType == 1) {
					for (int i = 0; i < angles.Length; i += 2) {
						GameObject tempArrow = (GameObject)(Instantiate (arrow, firePoint.position, Quaternion.Euler (new Vector3 (0, 0, angles [i] + 90))));
						ArrowController arrowCont = tempArrow.GetComponent<ArrowController> ();
						arrowCont.hVelocity = hVelocities [i];
						arrowCont.vVelocity = vVelocities [i];
					}
				} else {
					for (int i = 1; i < angles.Length; i+=2) {
						GameObject tempArrow = (GameObject)(Instantiate (arrow, firePoint.position, Quaternion.Euler (new Vector3 (0, 0, angles [i] + 90))));
						ArrowController arrowCont = tempArrow.GetComponent<ArrowController> ();
						arrowCont.hVelocity = hVelocities [i];
						arrowCont.vVelocity = vVelocities [i];
					}
				}
			}
			yield return new WaitForSeconds (4.0f);
		}
	}
}
