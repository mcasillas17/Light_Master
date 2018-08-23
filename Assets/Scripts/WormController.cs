using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour {

	public float moveSpeed;
	public bool moveRight;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	public bool grounded;

	public bool notAtEdge;
	public Transform edgeCheck;
	public Transform wallCheck;
	public bool hittingWall;

	public GameObject wormDamageParticles;
	public GameObject wormDeathParticles;

	public int currentLife;

	public Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		currentLife = 80;
	}
	
	// Update is called once per frame
	void Update () {

		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, groundCheckRadius, whatIsGround);
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, groundCheckRadius, whatIsGround);

		if (hittingWall || !notAtEdge) {
			moveRight = !moveRight;
		}
		if (moveRight) {
			transform.localScale = new Vector3 (-1f, 1f, 1f);
			rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
		} else {
			transform.localScale = new Vector3 (1f, 1f, 1f);
			rb.velocity = new Vector2 (-moveSpeed, rb.velocity.y);
		}
	}

	public void GetAttackFromPlayer(int damage){
		currentLife -= damage;
		if (currentLife > 0) {
			Instantiate (wormDamageParticles, transform.position, Quaternion.identity);
		} else {
			Instantiate (wormDeathParticles, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
