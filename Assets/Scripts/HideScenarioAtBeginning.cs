using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScenarioAtBeginning : MonoBehaviour {

	public Color targetColor;
	public float hideSpeed;

	void Update () {
		hideSpeed += 0.005f;
		RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, targetColor, hideSpeed * Time.deltaTime);
		if (RenderSettings.ambientLight == targetColor) {
			Destroy (gameObject);
		}
	}
}
