using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPlayerController : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D other){
		if (other.transform.tag == "Player") {
			other.gameObject.transform.parent = this.gameObject.transform;
		}
	}
	void OnCollisionExit2D(Collision2D other){
		if (other.transform.tag == "Player") {
			other.gameObject.transform.parent = null;
		}
	}

}
