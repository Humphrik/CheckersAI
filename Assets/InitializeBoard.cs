using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBoard : MonoBehaviour {

	const int BOARD_SIZE = 8;
	int[,] gridStates = new int[8, 8];

	public Transform redChecker, blueChecker = null;

	// Use this for initialization
	void Start () {
		//Red Pieces
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 4; j++) {
				int xAdjusted = 2*j + (i%2==0?0:1);
				float[] coords = CoordToWorldPosition(xAdjusted, i);
				Transform checker = GameObject.Instantiate(redChecker, new Vector3(coords[0], 0f, coords[1]),
				                                            redChecker.rotation);
				checker.GetComponent<CheckerPiece>().state = (int)BoardStates.States.Red;
				checker.GetComponent<CheckerPiece>().coords = new float[] {xAdjusted, i};
				gridStates[xAdjusted,j] = (int)BoardStates.States.Red;
			}
		}
		// Blue Pieces
		for (int i = 5; i < 8; i++) {
			for(int j = 0; j < 4; j++) {
				int xAdjusted = 2*j + (i%2==0?0:1);
				float[] coords = CoordToWorldPosition(xAdjusted, i);
				Transform checker = GameObject.Instantiate(blueChecker, new Vector3(coords[0], 0f, coords[1]),
				                       blueChecker.rotation);
				checker.GetComponent<CheckerPiece>().state = (int)BoardStates.States.Blue;
				checker.GetComponent<CheckerPiece>().coords = new float[] {xAdjusted, i};
				gridStates[xAdjusted,j] = (int)BoardStates.States.Blue;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float[] CoordToWorldPosition(int x, int y) {
		float xPos = 20 * x - 70;
		float yPos = 20 * y - 70;
		return new float[] {xPos, yPos};
	}

}
