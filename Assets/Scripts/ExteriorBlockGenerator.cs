using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ExteriorBlockGenerator : MonoBehaviour {

	public float blockHeight;
	public float blockWidth;

	public GameObject [] dirtTiles;
	public GameObject [] pikes;
	Vector3 roomPosition;
	public GameObject startPoint;
	public GameObject coinsSmall;

	public int height = 13;
	public int width = 23;
	public TextAsset roomTemplates;
	string [] templates;
	char [,] room;
	float tile_width, tile_height;

	string currentTemplate;

	private void getTemplates(){
		string txtRooms = roomTemplates.text;
		templates = Regex.Split(txtRooms,"\n");
	}

	private void setRoomTemplate(){
		currentTemplate = templates[Random.Range(0,templates.Length)];
		//Debug.Log (currentTemplate);
		int index = 0;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				room [i, j] = currentTemplate [index++];
			}
		}
	}

	public void createRoom(){
		Vector3 currentPosition = new Vector3(roomPosition.x,roomPosition.y,dirtTiles[0].transform.position.z);
		for (int i = 0; i < height; i++) {
			currentPosition = new Vector3 (roomPosition.x, roomPosition.y - 2 * tile_height * i, dirtTiles [0].transform.position.z);
			for (int j = 0; j < width; j++) {
				currentPosition.x += 2*tile_width;
				GameObject currentTile = null;
				if (room [i, j] == '1') { // solid tile
					int number = Random.Range (0, 100);
					if (number <= 4) {
						currentTile = (GameObject)Instantiate (dirtTiles [2], currentPosition, dirtTiles [0].transform.rotation);
					} else if (number <= 22) {
						currentTile = (GameObject)Instantiate (dirtTiles [1], currentPosition, dirtTiles [0].transform.rotation);
					} else {
						currentTile = (GameObject)Instantiate (dirtTiles [0], currentPosition, dirtTiles [0].transform.rotation);
					}
				} else if(room[i,j]>='2' && room[i,j]<='5'){ // probabilistic solid tile (no obstacle block)
					int numericValue = room [i, j] - '0';
					float probability = 1 / (float)numericValue;
					int maxValue = (int)(probability * 100);
					int rand = Random.Range (0, 100);
					if (rand <= maxValue) {
						int number = Random.Range (0, 100);
						if (number <= 4) {
							currentTile = (GameObject)Instantiate (dirtTiles [2], currentPosition, dirtTiles [0].transform.rotation);
						} else if (number <= 22) {
							currentTile = (GameObject)Instantiate (dirtTiles [1], currentPosition, dirtTiles [0].transform.rotation);
						} else {
							currentTile = (GameObject)Instantiate (dirtTiles [0], currentPosition, dirtTiles [0].transform.rotation);
						}
					}
				} else if (room [i, j] == 'S') {
					currentTile = (GameObject)Instantiate (startPoint, currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'P') {
					int pikeIdx = Random.Range (0, 2);
					currentTile = (GameObject)Instantiate (pikes [pikeIdx], currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'c') {
					currentTile = (GameObject)Instantiate (coinsSmall, currentPosition, Quaternion.identity);
				}
				if (currentTile != null) {
					currentTile.transform.SetParent (gameObject.transform);
				}
			}
		}
	}


	// Use this for initialization
	void Start () {
		Renderer rend = dirtTiles[0].GetComponent<Renderer> ();
		tile_width = rend.bounds.extents.x;
		tile_height = rend.bounds.extents.y;
		blockHeight = height * tile_height;
		blockWidth = width * tile_width;
		room = new char[height, width];
		roomPosition = transform.position;
		getTemplates ();
		setRoomTemplate ();
		createRoom ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
