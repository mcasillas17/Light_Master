using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TutorialGenerator : MonoBehaviour {

	public TutorialRoomGenerator tutorialRoom;
	public TutorialRoomGenerator[,] roomsObjects;
	string [] templates;
	public TextAsset roomTemplates;
	public GameObject dirtTile;
	public int height;
	public int width;
	float roomWidth;
	float roomHeight;
	public GameObject player;
	public GameObject startPoint;
	public GameObject backgroundTile;
	public LevelManager levelManager;

	private void getTemplates(){
		string txtRooms = roomTemplates.text;
		templates = Regex.Split(txtRooms,"\n");
	}

	void init(){
		roomsObjects = new TutorialRoomGenerator[2, 2];
		Renderer rend = dirtTile.GetComponent<Renderer> ();
		roomHeight = rend.bounds.extents.y*8;
		roomWidth = rend.bounds.extents.x*10;
		roomHeight *= 2;
		roomWidth *= 2;
		Vector3 currentPosition = transform.position;
		for (int i = 0; i < 2; i++) {
			currentPosition = new Vector3 (transform.position.x, transform.position.y-roomHeight*i, 0);
			for (int j = 0; j < 2; j++) {
				roomsObjects [i, j] = Instantiate (tutorialRoom, currentPosition, Quaternion.identity);
				roomsObjects [i, j].initRoom ();
				currentPosition.x += roomWidth;
			}
		}
	}

	void setTemplates(){
		int index = 0;
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				roomsObjects [i, j].setRoomTemplate (templates[index++]);
			}
		}
	}

	void createTutorial(){
		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 2; j++) {
				roomsObjects [i, j].createRoom ();
			}
		}
	}


	// Use this for initialization
	void Start () {
		init ();
		getTemplates ();
		setTemplates ();
		createTutorial ();
		player = GameObject.Find ("Player");
		FindObjectOfType<LevelManager>().currentCheckPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		startPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		player.transform.position = startPoint.transform.position;
	}

}
