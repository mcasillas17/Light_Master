using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckPoint;
	public LightMasterController player;
	public GameObject mainCam;

	public GameObject deathParticle;
	public GameObject respawnParticle;

    public BTEvolver wormEvolver;
    public List<WormBTController> wormObjects;

	public float gravityStore;
	public float respawnDelay;

	// Use this for initialization
	public void initManager () {
        wormObjects = new List<WormBTController>();
        player = FindObjectOfType<LightMasterController> ();
		currentCheckPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		respawnDelay = 1.5f;
		gravityStore = player.GetComponent<Rigidbody2D> ().gravityScale;
        wormEvolver = FindObjectOfType<BTEvolver>();
        wormEvolver.initEvolver();
	}

	public void respawnPlayer(){
		StartCoroutine ("respawnPlayerCorutine");
	}

	public IEnumerator respawnPlayerCorutine(){
        player = FindObjectOfType<LightMasterController>();
        Instantiate (deathParticle, player.gameObject.transform.position, Quaternion.identity);
		player.gameObject.SetActive (false);
		player.GetComponent<Renderer> ().enabled = false;
		//player.GetComponent<Rigidbody2D> ().gravityScale = 0.0f;
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;

		yield return new WaitForSeconds (respawnDelay);

		if (!player.isDead ()) {
			player.transform.position = new Vector3 (currentCheckPoint.transform.position.x, currentCheckPoint.transform.position.y, player.transform.position.z);
			mainCam.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, mainCam.transform.position.z);
			Instantiate (respawnParticle, player.transform.position, Quaternion.identity);
			player.gameObject.SetActive (true);
			player.GetComponent<Renderer> ().enabled = true;
            //player.GetComponent<Rigidbody2D>().gravityScale = gravityStore;
		} else {
            saveBTsFitness();
			string currentScene = SceneManager.GetActiveScene ().name;
			SceneManager.LoadScene (currentScene);
		}
	}

    public void addWorm(WormBTController worm){
        wormObjects.Add(worm);
    }

    public void saveBTsFitness(){
        for(int i = 0; i < wormObjects.Count; i++){
            wormObjects[i].disableWorm();
        }
        for (int i = 0; i < wormObjects.Count; i++) {
            int treeIndex = wormObjects[i].getTreeIndex();
            double fitness = wormObjects[i].getFinalFitness();
            wormEvolver.addFitnessToGenotype(treeIndex, fitness);
        }
        wormEvolver.finishEvolver();
    }

	public void MoveToNextLevel(){
        saveBTsFitness();
		string currentScene = SceneManager.GetActiveScene ().name;
		if (currentScene == "Tutorial") {
			SceneManager.LoadScene ("DirtLevel");
		} else if (currentScene == "DirtLevel") {
			SceneManager.LoadScene ("DarkLevel");
		} else if (currentScene == "DarkLevel") {
			SceneManager.LoadScene ("GrassLevel");
		} else if (currentScene == "GrassLevel") {
			SceneManager.LoadScene ("IceLevel");
		}else if (currentScene == "IceLevel") {
			SceneManager.LoadScene ("DirtLevel");
		}
	}

    public void enableBTOnWorms(){
        for(int i = 0; i < wormObjects.Count; i++){
            wormObjects[i].enableBT();
        }
    }
    
}
