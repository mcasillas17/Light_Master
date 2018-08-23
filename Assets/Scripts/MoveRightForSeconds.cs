using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MoveRightForSeconds : Action {

    public float duration;
    private Rigidbody2D rb;
    public float elapsedTime;
    public float moveSpeed;

    public override void OnStart(){
        duration = Random.Range(1.5f, 3.0f);
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        moveSpeed = 0.25f;
        elapsedTime = 0.0f;
    }

    public override TaskStatus OnUpdate(){
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (elapsedTime >= duration){
            return TaskStatus.Success;
        }
        else{
            this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            elapsedTime += Time.deltaTime;
            return TaskStatus.Running;
        }
    }
}
