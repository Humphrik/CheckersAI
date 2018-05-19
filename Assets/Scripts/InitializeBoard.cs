using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitializeBoard : MonoBehaviour {

	// Player Turns alternate between 1 (red) and -1 (blue).
	public int playerTurn = 1;

	// Important "locks" to allow for chaining of captures.
	bool chainLock = false;
	Transform chainChecker = null;


	// Your prefab checker models. Can be anything, but must have "CheckerPiece" script and 1 child (the crown).
	public Transform redChecker, blueChecker = null;

	// Everyone's a winner if they're having fun. (Visible on UI when someone wins.)
	public Text winText = null;


	void Start () {
		// Place Red Pieces on the board.
		for (int i = 0; i < 3; i++) {
			for(int j = 0; j < 4; j++) {
				int xAdjusted = 2*j + (i%2==0?0:1);
				float[] coords = CoordToWorldPosition(xAdjusted, i);
				Transform checker = GameObject.Instantiate(redChecker, new Vector3(coords[0], 0f, coords[1]),
				                                            redChecker.rotation);
				checker.GetComponent<CheckerPiece>().state = (int)BoardStates.States.Red;
				checker.GetComponent<CheckerPiece>().coords = new int[] {xAdjusted, i};
			}
		}
		// Place Blue Pieces on the board.
		for (int i = 5; i < 8; i++) {
			for(int j = 0; j < 4; j++) {
				int xAdjusted = 2*j + (i%2==0?0:1);
				float[] coords = CoordToWorldPosition(xAdjusted, i);
				Transform checker = GameObject.Instantiate(blueChecker, new Vector3(coords[0], 0f, coords[1]),
				                       blueChecker.rotation);
				checker.GetComponent<CheckerPiece>().state = (int)BoardStates.States.Blue;
				checker.GetComponent<CheckerPiece>().coords = new int[] {xAdjusted, i};
			}
		}
	}
	
	// Update listens for clicks, and checks if a new turn is ready.
	void Update(){

		// For chaining together captures. **Player is forced to chain!**
		if (chainLock && chainChecker.GetComponent<CheckerPiece> ().numValidMoves == 0) {
			// If there are no more valid, chained moves, the game moves on to the other player's turn.
			chainLock = false;
			playerTurn *= -1;
			foreach (GameObject highlights in GameObject.FindGameObjectsWithTag("Highlight"))
				GameObject.Destroy (highlights);
			CheckWinner();
		}

		if (Input.GetMouseButtonDown(0)){
			// Raycast from mouse on to field.
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				// Clicked on a checker? Show potential moves.
				if (hit.transform.gameObject.CompareTag ("GamePiece")) {
					if (chainLock)
						// No selecting new checkers if there's chaining to be done!
							return;
					int hitState = hit.transform.GetComponent<CheckerPiece> ().state;
					// It's not your turn, cheater.
					if ((hitState == 1 || hitState == 3) && playerTurn == -1
					    || (hitState == 2 || hitState == 4) && playerTurn == 1)
						return;
					// Important: this line places highlight for potential moves for that checker.
					hit.transform.gameObject.GetComponent<CheckerPiece> ().CheckValidMoves (false);
				// Clicked a highlight? Confirm your move!
				} else if (hit.transform.gameObject.CompareTag ("Highlight")) {
					hit.transform.GetComponent<Highlight> ().ConfirmMove ();
					chainChecker = hit.transform.GetComponent<Highlight> ().checkerPiece;
					// Activate the lock. Unlocks when ready to move onto next turn.
					chainLock = true;
				}
			}
		}
	}

	float[] CoordToWorldPosition(int x, int y) {
		// Translates from an 8x8 grid state to an actual place in the world.
		// **IMPORTANT: Y-AXIS ON THE GRID IS Z-AXIS IN THE GAME WOLRD.**
		float xPos = 20 * x - 70;
		float yPos = 20 * y - 70;
		return new float[] {xPos, yPos};
	}

	public void CheckWinner() {
		// Counts remaining pieces. Has somebody run out?
		int redCount = 0, blueCount = 0;
		foreach(GameObject checker in GameObject.FindGameObjectsWithTag("GamePiece")) {
			if (checker.GetComponent<CheckerPiece> ().state % 2 == 1)
				redCount++;
			else
				blueCount++;
		}
		if (redCount == 0)
			winText.text = "Blue Wins!";
		else if (blueCount == 0)
			winText.text = "Red Wins!";


	}



}
