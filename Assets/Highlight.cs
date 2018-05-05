using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

	public Transform checkerPiece = null;
	public Transform capturePiece = null;
	public int[] coords = new int[2];

	public void ConfirmMove() {
		checkerPiece.transform.position = transform.position + 0.01f*Vector3.down;
		CheckerPiece checkerData = checkerPiece.GetComponent<CheckerPiece> ();
		checkerData.coords = coords;
		if (capturePiece != null) {
			GameObject.Destroy (capturePiece.gameObject);
		}
	}
}
