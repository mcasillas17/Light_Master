using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMapController : MonoBehaviour {

	public bool moving;
	Transform targetPosition;
	public float speed;
	public MapObjectController currentPoint;
	// 0 - right
	// 1 - down
	// 2 - left
	// 3 - up
	MapObjectController [] currentNeighbors;

	void Start () {
		GameObject startPoint = GameObject.Find ("StartPoint");
		transform.position = startPoint.transform.position;
		currentPoint = startPoint.GetComponent<MapObjectController> ();
		currentNeighbors = currentPoint.neighbors;
		moving = false;
	}

	void Update () {
		if (moving) {
			if (Vector2.Distance (transform.position, targetPosition.position) < 0.02f) {
				transform.position = targetPosition.position;
				moving = false;
			} else {
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, targetPosition.position, step);
			}
		} else {
			if (Input.GetButtonDown ("StartLevel")) {
				if(currentPoint.isLevel){
					SceneManager.LoadScene (currentPoint.levelName);
				}
			} else if (Input.GetAxis ("Horizontal") > 0.5f) {
				if (currentNeighbors [0] != null) { // move right
					targetPosition = currentNeighbors[0].gameObject.transform;
					currentPoint = currentNeighbors [0];
					currentNeighbors = currentPoint.neighbors;
					moving = true;
				}
			} else if (Input.GetAxis ("Horizontal") < -0.5f) {
				if (currentNeighbors [2] != null) { // move left
					targetPosition = currentNeighbors[2].gameObject.transform;
					currentPoint = currentNeighbors [2];
					currentNeighbors = currentPoint.neighbors;
					moving = true;
				}
			}else if (Input.GetAxis ("Vertical") > 0.5f) {
				if (currentNeighbors [3] != null) { // move up
					targetPosition = currentNeighbors[3].gameObject.transform;
					currentPoint = currentNeighbors [3];
					currentNeighbors = currentPoint.neighbors;
					moving = true;
				}
			} else if (Input.GetAxis ("Vertical") < -0.5f) {
				if (currentNeighbors [1] != null) { // move down
					targetPosition = currentNeighbors[1].gameObject.transform;
					currentPoint = currentNeighbors [1];
					currentNeighbors = currentPoint.neighbors;
					moving = true;
				}
			}
		}
	}

}
