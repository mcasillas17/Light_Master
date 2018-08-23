using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlayerAtBeginning : MonoBehaviour {

	public Light characterLight;
	public Light bodyLight;
	public float iluminateSpeed;
	public float targetCharacterLight;
	public float targetBodyLight;

	// Use this for initialization
	void Start () {
		characterLight = GameObject.Find ("CharacterLight").GetComponent<Light>();
		bodyLight = GameObject.Find ("BodyLight").GetComponent<Light>();
		characterLight.intensity = 0f;
		bodyLight.intensity = 0f;
		targetCharacterLight = 1.6f;
		targetBodyLight = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		characterLight.intensity = Mathf.Lerp (characterLight.intensity, targetCharacterLight, iluminateSpeed * Time.deltaTime);
		bodyLight.intensity = Mathf.Lerp (bodyLight.intensity, targetBodyLight, iluminateSpeed * Time.deltaTime);
		if (characterLight.intensity == targetCharacterLight && bodyLight.intensity == targetBodyLight) {
			Destroy (gameObject);
		}
	}
}
