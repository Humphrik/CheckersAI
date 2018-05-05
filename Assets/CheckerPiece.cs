using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckerPiece : MonoBehaviour{

	public Transform highlight = null;

	public int state = (int)BoardStates.States.None;
	public int[] coords = new int[2];
	public static Vector3[] diagonals = new Vector3[] {
		Vector3.forward + Vector3.left,
		Vector3.forward + Vector3.right,
		Vector3.back + Vector3.right,
		Vector3.back + Vector3.left };
	public static int[,] diagonalCoords = new int[,] {{-1,1}, {1,1}, {1,-1}, {-1,-1}};


	public void CheckValidMoves() {
		// Debug.Log ("(" + coords[0] + "," + coords[1] + ")");
		for (int i = 0; i <= 3; i++) {
			if((state == (int)BoardStates.States.Red && i >= 2) 
			   || (state == (int)BoardStates.States.Blue && i < 2))
				continue;

			RaycastHit hit, hit2;
			if (Physics.Raycast (transform.position, diagonals[i] , out hit, 32)) {
				int hitState = hit.transform.GetComponent<CheckerPiece> ().state;
				if ((state == 1 || state == 3) && (hitState == 2 || hitState == 4)
				    || (state == 2 || state == 4) && (hitState == 1 || hitState == 3)) {
					// ***^It's hard to keep using the enum, okay?^***
					if (!Physics.Raycast (hit.transform.position, diagonals [i], out hit2, 32)) {
						int newX = coords[0] + 2 * diagonalCoords[i,0];
						int newY = coords[1] + 2 * diagonalCoords[i,1];

						if (IsValidPosition (newX, newY)) {
							// A valid capture!
							// Debug.Log ("Valid Space at: " + "(" + newX + ", " + newY + ")");
							float[] worldCoords = CoordToWorldPosition (newX, newY);
							Transform potentialMove = GameObject.Instantiate (highlight, new Vector3 (worldCoords [0], 0.1f, worldCoords [1]), highlight.rotation);
							Highlight highlightData = potentialMove.GetComponent<Highlight> ();
							highlightData.checkerPiece = transform;
							highlightData.capturePiece = hit.transform;
							highlightData.coords = new int[] {newX, newY};
								
						}
					}

				}


			}
			else {
				int newX = coords[0] + diagonalCoords[i,0];
				int newY = coords[1] + diagonalCoords[i,1];

				if (IsValidPosition (newX, newY)) {
					// A conventional move.
					// Debug.Log ("Valid Space at: " + "(" + newX + ", " + newY + ")");
					float[] worldCoords = CoordToWorldPosition (newX, newY);
					Transform potentialMove = GameObject.Instantiate (highlight, new Vector3 (worldCoords [0], 0.1f, worldCoords [1]), highlight.rotation);
					Highlight highlightData = potentialMove.GetComponent<Highlight> ();
					highlightData.checkerPiece = transform;
					highlightData.coords = new int[] {newX, newY};
				}
			}
		}
	}

	public bool IsValidPosition(int x, int y) {
		return x >= 0 && x <= 7 && y >= 0 && y <= 7;
	}

	public float[] CoordToWorldPosition(int x, int y) {
		float xPos = 20 * x - 70;
		float yPos = 20 * y - 70;
		return new float[] {xPos, yPos};
	}

	public int[] WorldToCoordPosition(float x, float y) {
		float xPos = (x + 70)/20;
		float yPos = (y + 70)/20;
		return new int[] {(int)(xPos + 0.5f), (int)(yPos + 0.5f)};
	}
}
