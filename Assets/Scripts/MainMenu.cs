using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame(){
		SceneManager.LoadScene ("DirtLevel");
	}

	public void QuitGame(){
		Application.Quit ();
	}

    public void setEvolverMode(bool value){
        if(value){
            PlayerPrefs.SetInt("evolverMode", 0);
        }
        else{
            PlayerPrefs.SetInt("evolverMode", 1);
        }
        Debug.Log("Currrent mode: " + PlayerPrefs.GetInt("evolverMode"));
    }
}
