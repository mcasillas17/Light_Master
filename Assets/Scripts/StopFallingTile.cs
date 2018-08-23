using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFallingTile : MonoBehaviour {

	public GameObject fallingTileParticle;

	void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Tile" || other.gameObject.tag=="Enemy" || other.gameObject.tag=="BreakableBlock") {
			Instantiate (fallingTileParticle, gameObject.transform.parent.transform.position, Quaternion.identity);
			/*foreach (Transform child in transform.parent.transform) {
				if (child.CompareTag ("Light")) {
					child.transform.parent = null;
				}
			}*/
			Destroy (gameObject.transform.parent.gameObject);
		}
	}
}
