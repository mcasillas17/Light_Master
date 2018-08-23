using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour {

	public float fillAmount;
	public float lerpSpeed;
	public Image content;
	public Text txtHealth;

	public float MaxValue{ get; set; }

	public float Value{
		set{ 
			txtHealth.text = "Light: " + value + " / " + MaxValue; 
			fillAmount = Map (value, MaxValue);
		}
	}

	// Use this for initialization
	void Start () {
		this.MaxValue = 150;
		this.Value = 150;
	}
	
	// Update is called once per frame
	void Update () {
		HandleBar ();
	}

	void HandleBar(){
		if (fillAmount != content.fillAmount) {
			content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
		}
	}

	float Map(float value, float maxValue){
		return value / maxValue;
	}
}
