using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTile : MonoBehaviour {

	Rigidbody2D rb;

	void Awake () {
		rb = GetComponentInParent<Rigidbody2D> ();
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			rb.isKinematic = false;
			rb.gravityScale = 1.5f;
		}
	}
}
