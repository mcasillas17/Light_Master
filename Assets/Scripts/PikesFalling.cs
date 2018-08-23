using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikesFalling : MonoBehaviour {

	public LevelManager levelManager;
	public int coinPenalty;
	public int damageToPlayer;
    public int damageToEnemy;

	void Start(){
		levelManager = FindObjectOfType<LevelManager> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			levelManager.respawnPlayer ();
			other.gameObject.GetComponent<LightMasterController> ().setDamage (damageToPlayer);
			CoinsManager.addCoins (-coinPenalty);
		}
        if(other.gameObject.tag=="Enemy"){
            other.gameObject.GetComponent<WormBTController>().getDamageFromFallingTile(damageToEnemy);
        }
	}
}
