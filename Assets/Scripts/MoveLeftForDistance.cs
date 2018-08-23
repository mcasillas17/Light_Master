using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class MoveLeftForDistance : Action {

    private float startX;
    private Rigidbody2D rb;
    public float elapsedTime;
    public float toleranceTime;
    private float distance;
    public float moveSpeed;

    public override void OnStart(){
        startX = this.gameObject.transform.position.x;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        moveSpeed = 0.25f;
        toleranceTime = Random.Range(5.0f, 7.0f);
        distance = Random.Range(3.0f, 7.0f);
        elapsedTime = 0.0f;
	}

    public override TaskStatus OnUpdate() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        float currentX = this.gameObject.transform.position.x;
        if(Mathf.Abs(currentX-startX)>=distance || elapsedTime>=toleranceTime){
            return TaskStatus.Success;
        }else{
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            elapsedTime += Time.deltaTime;
            return TaskStatus.Running;
        }
	}
}
