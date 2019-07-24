using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class RoomGenerator : MonoBehaviour {

	public int roomType;

	public float roomHeight;
	public float roomWidth;

    LevelManager levelManager;
    BTEvolver wormEvolver;

	public GameObject [] dirtTiles;
	public GameObject [] pikes;
	public GameObject [] knifeLaunchers;
	Vector3 roomPosition;
	public GameObject startPoint;
	public GameObject coinsSmall;
	public GameObject fallingTile;
	public GameObject breakableBlock;
	public GameObject entrance;
	public GameObject exit;
	public GameObject checkPoint;
	public GameObject wormEnemy;

	public int height = 8;
	public int width = 10;
	public TextAsset roomTemplates;
	public TextAsset groundObstacleTemplates;
	public TextAsset airObstacleTemplates;
	string [] templates;
	string [] groundTemplates;
	string [] airTemplates;
	char [,] room;
	float tile_width, tile_height;

	string currentTemplate;

	private void shuffleTemplates(){
		for (int k = 0; k < 10; k++) {
			for (int i = 0; i < templates.Length; i++) {
				int rand = Random.Range (0, templates.Length);
				string aux = templates [rand];
				templates [rand] = templates [i];
				templates [i] = aux;
			}
			for (int i = 0; i < groundTemplates.Length; i++) {
				int rand = Random.Range (0, groundTemplates.Length);
				string aux = groundTemplates [rand];
				groundTemplates [rand] = groundTemplates [i];
				groundTemplates [i] = aux;
			}

			for (int i = 0; i < airTemplates.Length; i++) {
				int rand = Random.Range (0, airTemplates.Length);
				string aux = airTemplates [rand];
				airTemplates [rand] = airTemplates [i];
				airTemplates [i] = aux;
			}
		}
	}

	// Method that sets all the values for the templates used in
	// the current room
	private void getTemplates(){
		string txtRooms = roomTemplates.text;
		templates = Regex.Split(txtRooms,"\n");
		string txtGroundTemplates = groundObstacleTemplates.text;
		groundTemplates = Regex.Split (txtGroundTemplates, "\n");
		string txtAirTemplates = airObstacleTemplates.text;
		airTemplates = Regex.Split (txtAirTemplates, "\n");
		shuffleTemplates ();
	}

	private string getRandomGroundOstacle(){
		return groundTemplates[Random.Range(0,groundTemplates.Length)];
	}

	private string getRandomAirOstacle(){
		return airTemplates[Random.Range(0,airTemplates.Length)];
	}

	// Method that obtains a room template and sets it to
	// the character matrix that represents the level
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

	void setObstacleBlock(int i, int j, string template){
		int c_i = i;
		int c_j = j;
		int index = 0;
		for (int k = 0; k < 3; k++) {
			for (int t = 0; t < 5; t++) {
				room [c_i + k, c_j + t] = template [index++];
			}
		}
	}

	// Method that sets the ground Ostacles
	// the code for a start position is '5'
	//
	// for each character c in the template
	// 1/num(c) represents the probability of a solid tile
	private void setGroundObstacles(){
		string groundTemplate = getRandomGroundOstacle ();
		string resTemplate = "";
		for (int i = 0; i < groundTemplate.Length; i++) {
			if (groundTemplate [i] >= '0' && groundTemplate [i] <= '9') {
				int numericValue = groundTemplate [i] - '0';
				if (numericValue != 0) {
					float probability = 1 / (float)numericValue;
					int maxValue = (int)(probability * 100);
					int rand = Random.Range (0, 100);
					if (rand <= maxValue) {
						resTemplate += "1";
					} else {
						resTemplate += "0";
					}
				} else {
					resTemplate += "0";
				}
			} else {
				resTemplate += groundTemplate [i];
			}
		}
		for (int i = 0; i < height-3; i++) {
			for (int j = 0; j < width-5; j++) {
				if (room [i, j] == 'g') {
					setObstacleBlock (i, j,resTemplate);
				}
			}
		}
	}

	// Method that sets the Air Ostacles
	// the code for a start position is 'a'
	//
	// for each character c in the template
	// 1/num(c) represents the probability of a solid tile
	private void setAirObstacles(){
		string airTemplate = getRandomAirOstacle ();
		string resTemplate = "";
		for (int i = 0; i < airTemplate.Length; i++) {
			if (airTemplate [i] >= '0' && airTemplate [i] <= '9') {
				int numericValue = airTemplate [i] - '0';
				if (numericValue != 0) {
					float probability = 1 / (float)numericValue;
					int maxValue = (int)(probability * 100);
					int rand = Random.Range (0, 100);
					if (rand <= maxValue) {
						resTemplate += "1";
					} else {
						resTemplate += "0";
					}
				} else {
					resTemplate += "0";
				}
			} else {
				resTemplate += airTemplate [i];
			}
		}
		for (int i = 0; i < height-3; i++) {
			for (int j = 0; j < width-5; j++) {
				if (room [i, j] == 'a') {
					setObstacleBlock (i, j,resTemplate);
				}
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
					if (number <= 2) {
						currentTile = (GameObject)Instantiate (dirtTiles [0], currentPosition, Quaternion.identity);
					} else if (number <= 7) {
						currentTile = (GameObject)Instantiate (dirtTiles [1], currentPosition, Quaternion.identity);
					} else if (number <= 12) {
						currentTile = (GameObject)Instantiate (dirtTiles [2], currentPosition, Quaternion.identity);
					} else {
						int index = 0;
						int x = Random.Range (0, 100);
						if (x <= 80)
							index = 3;
						else if (x <= 90)
							index = 4;
						else
							index = 5;
						currentTile = (GameObject)Instantiate (dirtTiles [index], currentPosition, Quaternion.identity);
					}
				} else if (room [i, j] >= '2' && room [i, j] <= '5') { // probabilistic solid tile (no obstacle block)
					int numericValue = room [i, j] - '0';
					float probability = 1 / (float)numericValue;
					int maxValue = (int)(probability * 100);
					int rand = Random.Range (0, 100);
					if (rand <= maxValue) {
						int number = Random.Range (0, 100);
						if (number <= 4) {
							currentTile = (GameObject)Instantiate (dirtTiles [0], currentPosition, Quaternion.identity);
						} else if (number <= 10) {
							currentTile = (GameObject)Instantiate (dirtTiles [1], currentPosition, Quaternion.identity);
						} else if (number <= 22) {
							currentTile = (GameObject)Instantiate (dirtTiles [2], currentPosition, Quaternion.identity);
						} else {
							int index = 0;
							int x = Random.Range (0, 100);
							if (x <= 80)
								index = 3;
							else if (x <= 90)
								index = 4;
							else
								index = 5;
							currentTile = (GameObject)Instantiate (dirtTiles [index], currentPosition, Quaternion.identity);
						}
					}
				} else if (room [i, j] == 'S') {
					currentTile = (GameObject)Instantiate (startPoint, currentPosition, Quaternion.identity);
					Vector3 entrancePosition = currentPosition;
					entrancePosition.x += tile_width;
					entrancePosition.y -= tile_height;
					Instantiate (entrance, entrancePosition, Quaternion.identity);
				} else if (room [i, j] == 'E') {
					Vector3 exitPosition = currentPosition;
					exitPosition.x += tile_width;
					exitPosition.y -= tile_height;
					Instantiate (exit, exitPosition, Quaternion.identity);
				} else if (room [i, j] == 'P') {
					int pikeIdx = Random.Range (0, 2);
					currentTile = (GameObject)Instantiate (pikes [pikeIdx], currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'c') {
					currentTile = (GameObject)Instantiate (coinsSmall, currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'f') {
					currentTile = (GameObject)Instantiate (fallingTile, currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'b') {
					currentTile = (GameObject)Instantiate (breakableBlock, currentPosition, Quaternion.Euler (new Vector3 (0, 0, 90 * Random.Range (0, 4))));
				} else if (room [i, j] == 'k') {
					currentTile = (GameObject)Instantiate (knifeLaunchers [Random.Range (0, 3)], currentPosition, Quaternion.identity);
				} else if (room [i, j] == 'C') {
					currentTile = (GameObject)Instantiate (checkPoint, currentPosition, Quaternion.identity);
				} /*else if (room [i, j] == 'w') {
					currentTile = (GameObject)Instantiate (wormEnemy, currentPosition, Quaternion.identity);
                    int treeIndex = wormEvolver.getNextTreeIndex();
                    BTGenotype btGenotype = wormEvolver.getNextTree();
                    WormBTController controller = currentTile.GetComponent<WormBTController>();
                    levelManager.addWorm(controller);
                    controller.setBT(btGenotype, treeIndex);
				}*/
				if (currentTile != null) {
					currentTile.transform.SetParent (gameObject.transform);
				}
			}
		}
	}

	// Method that reflects the room over de y axis
	private void reflectRoom(){
		for (int i = 0; i < height; i++) {
			int l = 0;
			int r = width - 1;
			while (l < r) {
				char temp = room [i, l];
				room [i, l] = room [i, r];
				room [i, r] = temp;
				l++; r--;
			}
		}
	}

	public void setCoinsUsingDP(int coinsNumber){
		while (coinsNumber > 0) {
			int[,] S = new int[height, width];
			for (int i = 0; i < height; i++) {
				S [i, 0] = room [i, 0] == '0' ? 1 : 0;
			}
			for (int j = 0; j < width; j++) {
				S [0, j] = room [0, j] == '0' ? 1 : 0;
			}
			for (int i = 1; i < height; i++) {
				for (int j = 1; j < width; j++) {
					if (room [i, j] == '0') {
						S [i, j] = Mathf.Min (S [i, j - 1], Mathf.Min (S [i - 1, j], S [i - 1, j - 1])) + 1;
					} else {
						S [i, j] = 0;
					}
				}
			}
			int maxOfS = 0;
			int iOfMax = 0;
			int jOfMax = 0;
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					if (S [i, j] > maxOfS) {
						maxOfS = S [i, j];
						iOfMax = i;
						jOfMax = j;
					}
				}
			}
			for (int i = iOfMax; i > iOfMax - maxOfS && coinsNumber > 0; i--) {
				for (int j = jOfMax; j > jOfMax - maxOfS && coinsNumber > 0; j--) {
					room [i, j] = 'c';
					coinsNumber--;
				}
			}
		}
	}

	public void initRoom () {
        levelManager = FindObjectOfType<LevelManager>();
        wormEvolver = levelManager.wormEvolver;
		Renderer rend = dirtTiles[0].GetComponent<Renderer> ();
		tile_width = rend.bounds.extents.x;
		tile_height = rend.bounds.extents.y;
		roomHeight = height * tile_height;
		roomWidth = width * tile_width;
		room = new char[height,width];
		roomPosition = transform.position;
		getTemplates ();
		setRoomTemplate ();
		setGroundObstacles ();
		setAirObstacles ();
		int rand = Random.Range (0,100);
		if (rand < 50) {
			reflectRoom ();
		}
	}

	public void setStartPoint(){
		room [height - 1, 0] = room [height - 1, 1] = '1';
		room [height - 2, 0] = room [height - 2, 1] = '0';
		room [height - 3, 0] = 'S';
		room [height - 3, 1] = '0';
		for (int i = height - 4; i >= 0; i--) {
			if (room [i, 0] <= '1' || room [i, 0] >= '9')
				room [i, 0] = '0';
			if (room [i, 1] <= '1' || room [i, 1] >= '9')
				room [i, 1] = '0';
		}
	}

	public void setExitPoint(){
		room [height - 1, 0] = room [height - 1, 1] = '1';
		room [height - 2, 0] = room [height - 2, 1] = '0';
		room [height - 3, 0] = 'E';
		room [height - 3, 1] = '0';
		for (int i = height - 4; i >= 0; i--) {
			if (room [i, 0] <= '1' || room [i, 0] >= '9')
				room [i, 0] = '0';
			if (room [i, 1] <= '1' || room [i, 1] >= '9')
				room [i, 1] = '0';
		}
	}

	public void setCheckPoint(){
		room [height-2, width-3] = '0';
		room [height-2, width-2] = 'C';
		room [height-2, width-1] = '0';
		room [height-1, width-3] = '1';
		room [height-1, width-2] = '1';
		room [height-1, width-1] = '1';
	}

}
