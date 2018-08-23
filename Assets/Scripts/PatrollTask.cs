using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class PatrollTask : Action {

	public float moveSpeed;
	public bool moveRight;

	public LayerMask whatIsGround;
	public float checkRadius;
	public Transform wallCheck;
	public Transform edgeCheck;

	private bool hittingWall;
	private bool notAtEdge;

	private Rigidbody2D rb;

	public Transform getChildWithName(string name){
		foreach(Transform child in this.gameObject.transform){
			if (child.name == name ) {
				return child;
			}
		}
		return null;
	}

	public override void OnAwake(){
		wallCheck = getChildWithName ("WallCheck");
		edgeCheck = getChildWithName ("EdgeCheck");
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		checkRadius = 0.12f;
		moveSpeed = 0.25f;
        moveRight = this.gameObject.transform.localScale.x == -1.0f;
		whatIsGround = LayerMask.GetMask ("Ground");
	}

	public override TaskStatus OnUpdate(){
		hittingWall = Physics2D.OverlapCircle (wallCheck.position, checkRadius, whatIsGround);
		notAtEdge = Physics2D.OverlapCircle (edgeCheck.position, checkRadius, whatIsGround);
		if (hittingWall || !notAtEdge) {
			moveRight = !moveRight;
		}
		if (moveRight) {
			this.gameObject.transform.localScale = new Vector3 (-1f, 1f, 1f);
			rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
		} else {
			this.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
			rb.velocity = new Vector2 (-moveSpeed, rb.velocity.y);
		}
		return TaskStatus.Running;
	}
}
