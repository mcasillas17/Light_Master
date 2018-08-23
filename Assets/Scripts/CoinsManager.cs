using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsManager : MonoBehaviour {

	public static int coinsNumber;
	Text txtCoins;

	// Use this for initialization
	void Start () {
		txtCoins = GetComponent<Text> ();
		coinsNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (coinsNumber < 0) {
			coinsNumber = 0;
		}
		txtCoins.text = "x " + coinsNumber; 
	}

	public static void addCoins(int coinsToAdd){
		coinsNumber += coinsToAdd;
	}

	public static void resetCoins(){
		coinsNumber = 0;
	}
}
