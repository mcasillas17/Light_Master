using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlockController : MonoBehaviour {

	public Sprite[] imgs;
	public int currentLife;
	public int currentImg;
	public GameObject destroyParticles;
    public GameObject recoverLife;
	private SpriteRenderer rnd;

	void Start () {
		currentLife = 60;
		currentImg = 2;
		rnd = GetComponent<SpriteRenderer> ();
	}

	public void GetAttackFromPlayer (int damage) {
		currentLife -= damage;
		currentImg--;
		if (currentImg >= 0) {
			rnd.sprite = imgs [currentImg];
		} else {
			Instantiate (destroyParticles, transform.position, Quaternion.identity);
            //Instantiate(recoverLife, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}
}
