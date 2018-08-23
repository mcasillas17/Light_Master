using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {

	public LevelManager levelManager;
	public GameObject myLight;
	void Awake(){
		levelManager = FindObjectOfType<LevelManager> ();
		myLight.gameObject.SetActive (false);
		//myLight.SetActive (false);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			levelManager.currentCheckPoint = transform.gameObject;
			//myLight.SetActive (true);
			myLight.gameObject.SetActive (true);
		}
	}
}
