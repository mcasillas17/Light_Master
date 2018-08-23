using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

	public float MIN_X, MAX_X;
	public float MIN_Y, MAX_Y;

	// Update is called once per frame
	private void Update(){
		transform.position = new Vector3( Mathf.Clamp(transform.position.x, MIN_X, MAX_X), Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y), transform.position.z);
	}

}
