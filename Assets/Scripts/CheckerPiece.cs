using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckerPiece : MonoBehaviour{

	// Placeholder for marking potential moves.
	public Transform highlight = null;

	// Type of checker, and its position on the board respectively.
	public int state = (int)BoardStates.States.None;
	public int[] coords = new int[2];

	// For use in raycasting.
	public static Vector3[] diagonals = new Vector3[] {
		Vector3.forward + Vector3.left,
		Vector3.forward + Vector3.right,
		Vector3.back + Vector3.right,
		Vector3.back + Vector3.left };
	// How a piece will shift depending on the move chosen.
	public static int[,] diagonalCoords = new int[,] {{-1,1}, {1,1}, {1,-1}, {-1,-1}};

	// Important for chaining captures. When this is 0 AFTER A CHECK, no more chain-moves can be completed.
	public int numValidMoves = 0;

	// More like "CheckerValidMoves," Amirite?
	public void CheckValidMoves(bool isChain) {
		// Get rid of potential moves from previous selection.
			foreach (GameObject highlights in GameObject.FindGameObjectsWithTag("Highlight"))
				GameObject.Destroy(highlights);
		numValidMoves = 0;

		// For every possible direction, check for a valid move.
		for (int i = 0; i <= 3; i++) {
			// If you're not a king, you can't move backwards!
			if((state == (int)BoardStates.States.Red && i >= 2) 
			   || (state == (int)BoardStates.States.Blue && i < 2))
				continue;

			// First cast is from this script's parent. Second cast is from the piece the first cast hit (if applicable.)
			RaycastHit hit, hit2;
			// Did your first raycast hit a checker?
			if (Physics.Raycast (transform.position, diagonals [i], out hit, 32)) {
				int hitState = hit.transform.GetComponent<CheckerPiece> ().state;
				// Is the obstructing piece on the enemy team? (Using the enum is hard, okay?!?)
				if ((state == 1 || state == 3) && (hitState == 2 || hitState == 4)
				    || (state == 2 || state == 4) && (hitState == 1 || hitState == 3)) {
					// Did your second raycast NOT hit a checker?
					if (!Physics.Raycast (hit.transform.position, diagonals [i], out hit2, 32)) {
						// Your potential landing position.
						int newX = coords [0] + 2 * diagonalCoords [i, 0];
						int newY = coords [1] + 2 * diagonalCoords [i, 1];

						if (IsValidPosition (newX, newY)) {
							// A valid capture! Mark the move with a highlight!
							float[] worldCoords = CoordToWorldPosition (newX, newY);
							Transform potentialMove = GameObject.Instantiate (highlight, new Vector3 (worldCoords [0], 0.1f, worldCoords [1]), highlight.rotation);
							Highlight highlightData = potentialMove.GetComponent<Highlight> ();
							highlightData.checkerPiece = transform;
							// Note how the piece to be captured is passed to the highlight, as well.
							highlightData.capturePiece = hit.transform;
							highlightData.coords = new int[] { newX, newY };
							numValidMoves++;
						}
					}
				}
			}
			// Did your first raycast NOT hit?
			else {
				int newX = coords[0] + diagonalCoords[i,0];
				int newY = coords[1] + diagonalCoords[i,1];
				if (IsValidPosition (newX, newY) && !isChain) {
					// A conventional move. Mark it with a highlight.
					float[] worldCoords = CoordToWorldPosition (newX, newY);
					Transform potentialMove = GameObject.Instantiate (highlight, new Vector3 (worldCoords [0], 0.1f, worldCoords [1]), highlight.rotation);
					Highlight highlightData = potentialMove.GetComponent<Highlight> ();
					highlightData.checkerPiece = transform;
					highlightData.coords = new int[] {newX, newY};
					numValidMoves++;
				}
			}
		}
	}

	// Checks if you're still on the board.
	public bool IsValidPosition(int x, int y) {
		return x >= 0 && x <= 7 && y >= 0 && y <= 7;
	}

	// Same as in InitializeBoard. (I should probably make that one static and call it here tbh.)
	public float[] CoordToWorldPosition(int x, int y) {
		float xPos = 20 * x - 70;
		float yPos = 20 * y - 70;
		return new float[] {xPos, yPos};
	}

	// Unused. Does the inverse of CoordToWorldPosition. A total cop-out.
	public int[] WorldToCoordPosition(float x, float y) {
		float xPos = (x + 70)/20;
		float yPos = (y + 70)/20;
		return new int[] {(int)(xPos + 0.5f), (int)(yPos + 0.5f)};
	}

	// Here is your crown; your checkered majesty.
	public void KingMe() {
		state += 2;
		transform.GetChild (0).gameObject.SetActive (true);
	}
}
