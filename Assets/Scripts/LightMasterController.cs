using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMasterController : MonoBehaviour {

	public GameObject character_light;
	public GameObject body_light;
	public bool isLightEnabled;
	public Transform firePoint;
	public GameObject light_ball;
	public List<GameObject> balls;
	public LevelManager levelManager;
	public bool isOnExit;
	public GameObject exitParticle;

	private PlayerHealthController health;

	void Awake(){
		health = GetComponent<PlayerHealthController> ();
		health.Initialize ();
	}
		
	void Start () {
		balls = new List<GameObject> ();
		levelManager = FindObjectOfType<LevelManager> ();
	}
		

	void Update () {
		if (Input.GetButtonDown("SwitchPlayerLight")) {
			isLightEnabled = !isLightEnabled;
			character_light.SetActive (isLightEnabled);
			body_light.SetActive (isLightEnabled);
		}

		if (Input.GetButtonDown ("FireLightBall") && health.canShootBall()) {
			if (Input.GetAxis ("Vertical")>0.35f) {
				shootBallUp ();
			} else if (Input.GetAxis ("Vertical")<-0.35f) {
				shootBallDown ();
			}
			else shootBallHorizontal ();
		}

		if (Input.GetButtonDown ("GetLightBall")) {
			if (balls.Count > 0) {
				//while(balls.Count > 0 && balls[0]==null) balls.RemoveAt (0);
				balls [0].GetComponent<LightBallController> ().ball_state = 2;
				balls [0].transform.parent = null;
				balls.RemoveAt (0);
			}
		}

		if (isOnExit && Input.GetAxis ("Vertical")>0.3f) {
			StartCoroutine (MoveToNextLevel ());
		}

	}

	IEnumerator MoveToNextLevel(){
		GetComponent<SpriteRenderer> ().enabled = false;
		Instantiate (exitParticle, transform.position, Quaternion.identity);
		yield return new WaitForSeconds (1.2f);
		levelManager.MoveToNextLevel ();
	}

	void shootBallUp(){
		GameObject light_ball_temp = (GameObject)Instantiate (light_ball, firePoint.position, firePoint.rotation);
		LightBallController controller = light_ball_temp.GetComponent<LightBallController> (); 
		controller.shoot_speed_horizontal = 0f;
		balls.Add (light_ball_temp);
		health.decrementLifeOnShootBall ();
	}

	void shootBallDown(){
		GameObject light_ball_temp = (GameObject)Instantiate (light_ball, firePoint.position, firePoint.rotation);
		LightBallController controller = light_ball_temp.GetComponent<LightBallController> (); 
		controller.shoot_speed_vertical *= -1;
		controller.shoot_speed_horizontal = 0f;
		balls.Add (light_ball_temp);
		health.decrementLifeOnShootBall ();
	}

	void shootBallHorizontal(){
		GameObject light_ball_temp = (GameObject)Instantiate (light_ball, firePoint.position, firePoint.rotation);
		LightBallController controller = light_ball_temp.GetComponent<LightBallController> (); 
		if (transform.localScale.x < 0) {
			controller.shoot_speed_horizontal *= -1;
		}
		controller.shoot_speed_vertical = 0f;
		balls.Add (light_ball_temp);
		health.decrementLifeOnShootBall ();
	}

	public void recoverBall(){
		health.incrementLifeOnRecoverBall ();
	}

	public void setDamage(int damageAmount){
		health.decrementLife (damageAmount);
		if (health.currentLife <= 0) {
			health.currentLife = 0;
		}
	}

	public bool isDead(){
		return health.currentLife == 0;
	}

    public void incrementLife(int increment){
        health.incrementLife(increment);
    }
}
