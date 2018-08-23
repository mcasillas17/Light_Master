using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			LightMasterController controller = other.gameObject.GetComponent<LightMasterController> ();
			controller.isOnExit = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			LightMasterController controller = other.gameObject.GetComponent<LightMasterController> ();
			controller.isOnExit = false;
		}
	}
}
