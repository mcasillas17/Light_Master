using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Jump : Action {

    public LayerMask whatIsGround;
    public float checkRadius;
    public Transform groundCheck;
    public float jumpForce;
    public bool grounded;
    private Rigidbody2D rb;

    public Transform getChildWithName(string name){
        foreach (Transform child in this.gameObject.transform){
            if (child.name == name){
                return child;
            }
        }
        return null;
    }

    public override void OnStart(){
        whatIsGround = LayerMask.GetMask("Ground");
        groundCheck = getChildWithName("GroundCheck");
        checkRadius = 0.12f;
        jumpForce = 150f;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    public override TaskStatus OnUpdate(){
        grounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (grounded){
            rb.AddForce(new Vector2(0,jumpForce));
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}
