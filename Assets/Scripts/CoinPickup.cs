using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour {

	public int coinsToAdd;
	public GameObject coinParticle;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			gameObject.SetActive (false);
			Destroy (gameObject);
			Instantiate (coinParticle, this.gameObject.transform.position, Quaternion.identity);
			CoinsManager.addCoins (coinsToAdd);
		}
	}
}
