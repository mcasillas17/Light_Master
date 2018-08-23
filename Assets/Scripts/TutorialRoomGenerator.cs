using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TutorialRoomGenerator : MonoBehaviour {

	public float roomHeight;
	public float roomWidth;

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
	char [,] room;
	float tile_width, tile_height;

	public void setRoomTemplate(string template){
		int index = 0;
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				room [i, j] = template [index++];
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
				} else if (room [i, j] == 'w') {
					currentTile = (GameObject)Instantiate (wormEnemy, currentPosition, Quaternion.identity);
				}
				if (currentTile != null) {
					currentTile.transform.SetParent (gameObject.transform);
				}
			}
		}
	}

	public void initRoom () {
		Renderer rend = dirtTiles[0].GetComponent<Renderer> ();
		tile_width = rend.bounds.extents.x;
		tile_height = rend.bounds.extents.y;
		roomHeight = height * tile_height;
		roomWidth = width * tile_width;
		room = new char[height,width];
		roomPosition = transform.position;
	}
}
