using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

	public int angle;
	public float hVelocity;
	public float vVelocity;
	public int damageToPlayer;
	public int coinPenalty;
	public LevelManager levelManager;
	public int state;

	public GameObject destroyArrowParticle;
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		levelManager = FindObjectOfType<LevelManager> ();
		state = 0;
	}

	void Update () {
		if(state==0)
			rb.velocity = new Vector2 (hVelocity, vVelocity);
	}

	bool crashes(string otherTag){
        return otherTag=="Tile" || otherTag == "Obstacle" || otherTag == "BreakableBlock";
	}

	void OnTriggerEnter2D(Collider2D other){
		if (crashes(other.tag)) {
			rb.velocity = new Vector2 (0, 0);
			Instantiate (destroyArrowParticle, transform.position, transform.rotation);
			Destroy (gameObject);
		}
		if (other.tag == "Player") {
			levelManager.respawnPlayer ();
			other.gameObject.GetComponent<LightMasterController> ().setDamage (damageToPlayer);
			Instantiate (destroyArrowParticle, transform.position, transform.rotation);
			CoinsManager.addCoins (-coinPenalty);
			Destroy (gameObject);
		}
	}
}
