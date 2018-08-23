using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletController : MonoBehaviour {

    private Rigidbody2D rb;
    public int damageToPlayer;
    public float shoot_speed_horizontal = 4.0f;

    public WormBTController worm;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        rb.velocity = new Vector2(shoot_speed_horizontal, 0);
	}

    private bool crashes(string otherTag){
        return otherTag == "Tile" || otherTag == "Obstacle" || otherTag == "ArrowLauncher" || otherTag == "BreakableBlock";
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Player"){
            worm.incrementBulletsConnected();
            other.gameObject.GetComponent<LightMasterController>().setDamage(damageToPlayer);
            Destroy(gameObject);
        }
        else if (crashes(other.tag))
        {
            Destroy(gameObject);
        }
    }
}
