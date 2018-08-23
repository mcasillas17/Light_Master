using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBallController : MonoBehaviour {

	// 0 - shooting
	// 1 - in object
	// 2 - returning
	public int ball_state;
	public float shoot_speed_horizontal = 6.5f;
	public float shoot_speed_vertical = 6.5f;
	public float return_speed = 15.5f;
	private Rigidbody2D rb;
	private GameObject player;

	// Use this for initialization
	void Start () {
		ball_state = 0;
		rb = GetComponent<Rigidbody2D> ();
		player = GameObject.Find ("Player");
		GetComponent<SpriteRenderer>().color =  new Color (1f, 1f, 1f, 0.7f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ball_state == 0) {
			rb.velocity = new Vector2 (shoot_speed_horizontal, shoot_speed_vertical);
		}
		if (ball_state == 2) {
			if (player.activeSelf) {
				Vector2 direction = (player.transform.position - transform.position).normalized;
				rb.velocity = direction * return_speed;
			} else {
				rb.velocity = Vector2.zero;
			}
		}
	}

	bool crashes(string otherTag){
		return otherTag=="Tile" || otherTag == "Obstacle" || otherTag == "ArrowLauncher" || otherTag=="BreakableBlock";
	}

	void OnTriggerEnter2D(Collider2D other){
		if (crashes(other.tag) && ball_state==0) {
			ball_state = 1;
			rb.velocity = new Vector2 (0, 0);
			//transform.parent = other.gameObject.transform;
		}
		if (other.tag == "Player" && ball_state == 2) {
			other.gameObject.GetComponent<LightMasterController> ().recoverBall ();
			Destroy (gameObject);
		}
		if(other.tag == "ArrowLauncher" && ball_state==0) {
			ball_state = 1;
			rb.velocity = new Vector2 (0, 0);
			transform.parent = other.gameObject.transform;
		}
	}

}
