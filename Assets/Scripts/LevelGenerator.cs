using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public RoomGenerator[] rooms;
	public RoomGenerator[,] roomsObjects;
	public int height;
	public int width;
	int[,] roomTypes;
	bool[,] vis;
	public Vector3 startPosition;
	float roomWidth;
	float roomHeight;
	int i_start;
	int j_start;
	int i_end;
	int j_end;
	public GameObject player;
	public GameObject startPoint;
	public GameObject backgroundTile;
	public LevelManager levelManager;
	int[,] coinsInRooms;

	bool validCoordinate(int i, int j){
		return i >= 0 && i < height && j >= 0 && j < width;
	}

	void generateLevel(){
		int i = 0;
		int j = Random.Range (0, width);
		i_start = i;
		j_start = j;
		int prevRoom = 0;
		bool flag = true;
		while (flag) {
			if (!vis [i, j]) {
				roomTypes [i, j] = 1;
				vis [i, j] = true;
			}
			if (prevRoom == 2) {
				if (Random.Range (0, 100) < 15) {
					roomTypes [i, j] = 2;
				} else {
					roomTypes [i, j] = 3;
				}
				roomTypes [i, j] = 3;
			}
			int direction = Random.Range (1, 6);
			int ni = i; int nj = j;
			if (direction <= 2) {
				nj--;
			} else if (direction <= 4) {
				nj++;
			} else {
				if (i == height - 1) {
					flag = false;
					i_end = i;
					j_end = j;
				} else {
					ni++;
				}
			}
			if (flag && validCoordinate (ni, nj) && !vis [ni, nj]) {
				if (direction == 5) {
					roomTypes [i, j] = 2;
					prevRoom = 2;
				} else {
					prevRoom = roomTypes [i, j];
				}
				i = ni;
				j = nj;
			}
		}
	}

	void printLevel(){
		string level = "";
		for (int i = 0; i < height; i++) {
			string linea = "";
			for (int j = 0; j < width; j++) {
				linea += roomTypes [i, j];
			}
			level += linea+"\n";
		}
		print (level);
	}

	void createLevel(){
		Renderer rend = rooms [0].dirtTiles[0].GetComponent<Renderer> ();
		roomHeight = rend.bounds.extents.y*rooms[0].height;
		roomWidth = rend.bounds.extents.x*rooms[0].width;
		roomHeight *= 2;
		roomWidth *= 2;
		Vector3 currentPosition = startPosition;
		for (int i = 0; i < height; i++) {
			currentPosition = new Vector3 (startPosition.x, startPosition.y-roomHeight*i, 0);
			for (int j = 0; j < width; j++) {
				roomsObjects [i, j] = Instantiate (rooms [roomTypes [i, j]], currentPosition, Quaternion.identity);
				roomsObjects [i, j].initRoom ();
				currentPosition.x += roomWidth;
			}
		}
		roomsObjects [i_start, j_start].setStartPoint ();
		roomsObjects [i_end, j_end].setExitPoint ();
	}
	int getIndex(){
		int number = Random.Range (0, 100);
		int res = 0;
		if (number <= 70)
			res = 3;
		else if (number <= 80)
			res = 4;
		else
			res = 5;
		return res;
	}
	void setLevelBounds(){
		Renderer rend = rooms [0].dirtTiles[0].GetComponent<Renderer> ();
		float tileWidth = rend.bounds.extents.x;
		float tileHeight = rend.bounds.extents.y;
		tileWidth *= 2;
		tileHeight *= 2;
		startPosition.y += tileHeight;
		Vector3 currentPosition = startPosition;
		for (int i = 0; i < rooms [0].width * width + 2; i++) {
			Instantiate (rooms [0].dirtTiles[getIndex()], currentPosition, Quaternion.identity);
			for (int k = 0; k < 3; k++) {
				Vector3 otherTilePos = new Vector3 (currentPosition.x, currentPosition.y + k*tileHeight, currentPosition.z);
				Instantiate (rooms [0].dirtTiles[getIndex()], otherTilePos, Quaternion.identity);
			}
			currentPosition.x += tileWidth;
		}
		currentPosition.x -= tileWidth;
		for (int i = 0; i < rooms [0].height * height + 1; i++) {
			currentPosition.y -= tileHeight;
			Instantiate (rooms [0].dirtTiles[getIndex()], currentPosition, Quaternion.identity);
		}
		currentPosition = startPosition;
		for (int i = 0; i < rooms [0].height * height + 1; i++) {
			currentPosition.y -= tileHeight;
			Instantiate (rooms [0].dirtTiles[getIndex()], currentPosition, Quaternion.identity);
		}
		currentPosition.x += tileWidth;
		for (int i = 0; i < rooms [0].width * width; i++) {
			Instantiate (rooms [0].dirtTiles[getIndex()], currentPosition, Quaternion.identity);
			currentPosition.x += tileWidth;
		}
	}

	void instantiateLevel(){
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				roomsObjects [i, j].createRoom ();
			}
		}
		Renderer bgRend = backgroundTile.GetComponent<Renderer> ();
		float tile_width = bgRend.bounds.extents.x;
		float tile_height = bgRend.bounds.extents.y;
		tile_height *= 2;
		tile_width *= 2;
		Vector3 currentPosition = startPosition;
		for (int i = 0; i < height*2; i++) {
            currentPosition = new Vector3(startPosition.x - tile_width, startPosition.y - tile_height * i, 0);
			for (int j = 0; j < width*2; j++) {
				currentPosition.x += tile_width;
				Instantiate (backgroundTile, currentPosition, Quaternion.identity);
			}
		}
	}

	void setCoinsAmount(){
		coinsInRooms = new int[height, width];
		int numberOfRooms = 0;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (roomTypes [i, j] != 0) {
					numberOfRooms++;
				}	
			}
		}
		int coinsPerRoom = 9;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (roomTypes [i, j] != 0) {
					coinsInRooms [i, j] = Random.Range (3, coinsPerRoom + 1);
				}
			}
		}


		for (int i = 0; i < 20; i++) {
			int i1 = Random.Range (0, height);
			int j1 = Random.Range (0, width);
			int i2 = Random.Range (0, height);
			int j2 = Random.Range (0, width);
			if (coinsInRooms [i2, j2] > 0) {
				int coinsToMove = Random.Range (0, Mathf.Min (coinsInRooms [i1, j1] / 3, 3));
				coinsInRooms [i1, j1] -= coinsToMove;
				coinsInRooms [i2, j2] += coinsToMove;
			}
		}

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				roomsObjects [i, j].setCoinsUsingDP (coinsInRooms [i, j]);
			}
		}
	}

	void expandRoute(){
		for (int i = 0; i < height; i++) {
			int count = 0;
			for (int j = 0; j < width; j++) {
				if (roomTypes [i, j] != 0) {
					count++;
				}
			}

		}
	}

	void setCheckpoints(){
		List<int> rows = new List<int> ();
		List<int> cols = new List<int> ();
		int numberOfRooms = 0;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if (roomTypes [i, j] != 0) {
					rows.Add (i);
					cols.Add (j);
					numberOfRooms++;
				}	
			}
		}

		int howMany = rows.Count / 4;
		for (int i = howMany; i < rows.Count; i += howMany) {
			roomsObjects [rows [i], cols [i]].setCheckPoint ();
		}
	}

	bool isStartRoom(int i, int j){
		return i == i_start && j == j_start;
	}

	bool isEndRoom(int i, int j){
		return i == i_end && i == j_end;
	}

	void Start () {
		roomTypes = new int[height, width];
		vis = new bool[height,width];
		roomsObjects = new RoomGenerator[height, width];
        levelManager.initManager();
		generateLevel ();
		//printLevel ();
		createLevel ();
		setCheckpoints ();
		expandRoute ();
		setCoinsAmount ();

		//Instantiate (levelManager, Vector3.zero, Quaternion.identity);
		instantiateLevel ();
		setLevelBounds ();
		player = GameObject.Find ("Player");
		FindObjectOfType<LevelManager>().currentCheckPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		startPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		player.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		player.transform.position = startPoint.transform.position;
	}
}
