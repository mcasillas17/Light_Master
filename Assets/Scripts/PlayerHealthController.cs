using System.Collections;
using System;
using UnityEngine;

public class PlayerHealthController: MonoBehaviour{

	private HealthBarController bar;

	public GameObject characterLight;
	public GameObject bodyLight;

	public int currentLife;
	public int maxLife;

	public int lightBallAmountOnLife;
	public float lightBallAmountOnCharacter;
	public float lightBallAmountOnBody;
	public float originalIntensity;


	private float maxValue;
	private float currentValue;

	public float CurrentValue{
		get{
			return currentValue;
		}
		set{
			this.currentValue = value;
			bar.Value = currentValue;
		}
	}

	public float MaxValue{
		get{
			return maxValue;
		}
		set{
			this.maxValue = value;
			bar.MaxValue = maxValue;
		}
	}

	public void Initialize(){
		bar = FindObjectOfType<HealthBarController> ();
		this.MaxValue = maxLife;
		this.CurrentValue = currentLife;
	}

	void Start(){
		currentLife = 150;
		maxLife = 150;
		originalIntensity = characterLight.GetComponent<Light> ().intensity;
		lightBallAmountOnLife = 15;
		lightBallAmountOnCharacter = originalIntensity / 10;
		lightBallAmountOnBody = originalIntensity / 10;
	}

	public void decrementLifeOnShootBall(){
		currentLife -= lightBallAmountOnLife;
		this.CurrentValue = currentLife;
		decrementLightOnShootBall ();
	}

	public void incrementLifeOnRecoverBall(){
		currentLife += lightBallAmountOnLife;
		this.CurrentValue = currentLife;
		incrementLightOnRecoverBall ();
	}

	public void decrementLife(int amount){
		currentLife -= amount;
		this.CurrentValue = currentLife;
		if (currentLife <= 0) {
			this.CurrentValue = 0;
		}
		float newIntensity = characterLight.GetComponent<Light> ().intensity;
		newIntensity -= ((float)amount) / maxLife;
		if (newIntensity <= 0) {
			newIntensity = 0;
		}
		characterLight.GetComponent<Light> ().intensity = newIntensity;
	}

	public void incrementLife(int amount){
		currentLife += amount;
		this.CurrentValue = currentLife;
		if (currentLife >= this.MaxValue) {
			this.CurrentValue = this.MaxValue;
		}
		float newIntensity = characterLight.GetComponent<Light> ().intensity;
		newIntensity += ((float)amount) / maxLife;
		if (newIntensity >= originalIntensity) {
			newIntensity = originalIntensity;
		}
		characterLight.GetComponent<Light> ().intensity = newIntensity;
	}

	private void incrementLightOnRecoverBall(){
		characterLight.GetComponent<Light> ().intensity += lightBallAmountOnCharacter;
		bodyLight.GetComponent<Light> ().intensity += lightBallAmountOnBody;
	}

	private void decrementLightOnShootBall(){
		characterLight.GetComponent<Light> ().intensity -= lightBallAmountOnCharacter;
		bodyLight.GetComponent<Light> ().intensity -= lightBallAmountOnBody;
	}

	public bool canShootBall(){
		return currentLife > lightBallAmountOnLife;
	}
}
