using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour {

	public GameObject platform;
	public float speed;
	public Transform[] points;
	public int currentPoint;

	// Use this for initialization
	void Start () {
		if (speed == 0) {
			speed = Random.Range (0.7f, 2.2f);
		}
		currentPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		platform.transform.position = Vector3.MoveTowards(platform.transform.position, points[currentPoint].position, step);
		if (platform.transform.position == points [currentPoint].position) {
			currentPoint++;
			currentPoint %= points.Length;
		}
	}
}
