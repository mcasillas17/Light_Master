using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float maxSpeed;
	public float speed;
	public float jumpForce;
	public float doubleJumpForce;
	public bool isSimpleAttacking;
	public float simpleAttackTimer;
	public float simpleAttackCooldown;
	public Collider2D simpleAttackTrigger;

	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask whatIsGround;
	private bool grounded;
	private bool doubleJumped;
	private bool facingRight = true;

	private Rigidbody2D rb;
	private Animator anim;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		isSimpleAttacking = false;
		simpleAttackTimer = 0;
		simpleAttackCooldown = 0.25f;
		simpleAttackTrigger.enabled = false;
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		if (grounded) {
			doubleJumped = false;
		}
		float hMove = Input.GetAxis ("Horizontal");
		rb.velocity = new Vector2(hMove * maxSpeed, rb.velocity.y);
		//rb.AddForce (Vector2.right * hMove * speed);
		if (rb.velocity.x > maxSpeed) {
			rb.velocity = new Vector2 (maxSpeed, rb.velocity.y);
		}
		if (rb.velocity.x < -maxSpeed) {
			rb.velocity = new Vector2 (-maxSpeed, rb.velocity.y);
		}
		if (hMove > 0 && !facingRight) {
			Flip ();
		} else if (hMove < 0 && facingRight) {
			Flip ();
		}
	}

	void Update () {
		anim.SetFloat ("Speed", Mathf.Abs (Input.GetAxis ("Horizontal")));
		anim.SetBool ("Grounded", grounded);
		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (new Vector2 (0, jumpForce));
		}
		if (Input.GetButtonDown ("Jump") && !grounded && !doubleJumped) {
			if (rb.velocity.y < 0) {
				rb.velocity = new Vector2 (rb.velocity.x, 0);
				rb.AddForce (new Vector2 (0, doubleJumpForce));
			} else {
				rb.AddForce (new Vector2 (0, 5 * doubleJumpForce / 6));
			}
			doubleJumped = true;
		}
		if (Input.GetButtonDown ("SimpleAttack") && !isSimpleAttacking) {
			isSimpleAttacking = true;
			simpleAttackTimer = simpleAttackCooldown;
			simpleAttackTrigger.enabled = true;
		}
		if (isSimpleAttacking) {
			if (simpleAttackTimer > 0) {
				simpleAttackTimer -= Time.deltaTime;
			} else {
				isSimpleAttacking = false;
				simpleAttackTimer = 0;
				simpleAttackTrigger.enabled = false;
			}
		}
		anim.SetBool ("SimpleAttacking", isSimpleAttacking);
	}

	private void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
		
		
}
