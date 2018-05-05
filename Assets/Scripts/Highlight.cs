using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlight : MonoBehaviour {

	// Move data: The piece moved, the piece caputred (if applicable), the new position respectively.
	public Transform checkerPiece = null;
	public Transform capturePiece = null;
	public int[] coords = new int[2];

	public void ConfirmMove() {
		// Move the checker to the highlight's position in the world.
		checkerPiece.transform.position = transform.position + 0.01f*Vector3.down;
		CheckerPiece checkerData = checkerPiece.GetComponent<CheckerPiece> ();
		// Update its coordinates on the grid.
		checkerData.coords = coords;
		// King the checker if it reached the end and is one one already.
		if (coords [1] == 0 && checkerData.state == 2
			|| coords [1] == 7 && checkerData.state == 1)
			checkerData.KingMe ();

		// Did you just capture a piece?
		if (capturePiece != null) {
			// A small buffer, just in case.
			capturePiece.position = capturePiece.position + 10 * Vector3.down;
			// Brutally murder the captured piece.
			GameObject.Destroy (capturePiece.gameObject);
			// Check for potential chains.
			checkerData.CheckValidMoves (true);
		}
		else
			// Chaining is not possible if you did not capture a piece.
			checkerData.numValidMoves = 0;
	}
		
}
