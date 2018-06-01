using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxBase : MonoBehaviour {
	//All you have to do to activate the AI is change this boool
	public bool minimaxActive = false;

	//Creates the array board used for the tree
	//Just like some family trees, this tree has slaves and masters
	public int[,] board = new int[8,8];

	//Sets the max depth of the tree
	private int maxDepth = 1;

	public GameObject manager;

	public bool minimaxRunning = false;
	
	// If it's the correct turn and the AI is active, minimax is run
	void Update () {
		if (minimaxActive && manager.GetComponent<InitializeBoard>().playerTurn == -1&&minimaxRunning==false) {
			minimaxRunning = true;
			minimax();
		}
	}

	//Easier way of referring to the corners.
	public static int[,] diagonalCoords = new int[,] {{-1,1}, {-1,-1}, {1,-1}, {1,1}};

	void minimax (){
		Move bestMove = new Move();
		int bestScore = -9999;
		boardToArray();
		List<Move> legal = getLegalMoves(2);
		foreach (Move move in legal) {
			make (move);
			int score = min (0);
			if (score > bestScore) {
				bestScore = score;
				bestMove = move;
			}
			retract (move);
		}
		MakeMove (bestMove);
		manager.GetComponent<InitializeBoard> ().playerTurn = 1;
		minimaxRunning = false;
	}

	//Finds best move for AI
	int min (int depth){
		if (gg (2))
			return 1000;
		else if (depth == maxDepth) {
			return heval ();
		} else {
			int bestScore = 9999;
			List<Move> legal = getLegalMoves(2);
			foreach (Move move in legal){
				make (move);
				int score = max (depth+1);
				if (score<bestScore){
					bestScore=score;
				}
				retract (move);
			}
			return bestScore;
		}
	}

	//Finds best move for Human
	int max (int depth){
		if (gg (1))
			return -1000;
		else if (depth == maxDepth) {
			return heval ();
		} else {
			int bestScore = -9999;
			List<Move> legal = getLegalMoves(1);
			foreach (Move move in legal){
				make (move);
				int score = min (depth+1);
				if (score>bestScore){
					bestScore=score;
				}
				retract (move);
			}
			return bestScore;
		}
	}

	//Turns the physical board into an array that can be passed through the minimax
	void boardToArray () {
		for(int i=0; i<8; i++){
			for (int k = 0; k<8; k++){
				board [i,k] = 0;
			}
		}
		foreach (GameObject checker in GameObject.FindGameObjectsWithTag("GamePiece")) {
			board [checker.GetComponent<CheckerPiece>().coords [0],checker.GetComponent<CheckerPiece>().coords [1]] = checker.GetComponent<CheckerPiece>().state;
		}
		foreach (int con in board) {
			Debug.Log(con.ToString());
		}
	}

	//Gets the pieces for a team in format (x-coord, y-coord, state)
	List<int[]> getPieceCoords(int color){
		List<int[]> coords = new List<int[]> ();
		for (int i = 0; i < 8; i++) {
			for (int k = 0; k < 8; k++) {
				if (board [i,k] == color || board [i,k] == color + 2)
					coords.Add (new int[]{ i, k, board[i,k] });
			}
		}
		return coords;
	}

	//Gets a list of all legal moves
	List<Move> getLegalMoves(int color){
		List<int[]> coords = getPieceCoords (color);
		List<Move> moves = new List<Move> ();
		foreach (int[] piece in coords) {
			moves.AddRange(getAdjacentMoves (piece));
			moves.AddRange(getCaptures (piece));
		}
		return moves;
	}

	//Gets list of moves that can be made adjecently
	List<Move> getAdjacentMoves(int[] coords){
		List<Move> moves = new List<Move> ();
		if (coords [2] != 4) {
			for (int i = 0; i < 2; i++) {
				if (IsValidPosition(coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1])){	
					if (board [coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1]]==0) {
						int[] newEnd = new int[]{ coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1], coords [2] };
						Move adjacent = new Move (coords, newEnd);
						moves.Add (adjacent);
					}
				}
			}
		} 
		else {
			for (int i = 0; i < 4; i++) {
				if (IsValidPosition (coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1])) {
					if (board [coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1]] == 0) {
						int[] newEnd = new int[]{ coords [0] + diagonalCoords [i, 0], coords [1] + diagonalCoords [i, 1], coords [2] };
						Move adjacent = new Move (coords, newEnd);
						moves.Add (adjacent);
					}
				}
			}
		}
		return moves;
	}

	//Gets list of captures that can be made
	List<Move> getCaptures(int[] coords){
		int color = coords [2]%2;
		int oppo = (coords [2] + 1) % 2;
		List<Move> moves = new List<Move> ();
		for (int i=0; i<4; i++) {
			if(IsValidPosition(coords[0]+diagonalCoords[i,0], coords[1]+diagonalCoords[i,1])){
				if(board[coords[0]+diagonalCoords[i,0], coords[1]+diagonalCoords[i,1]]%2==oppo){
					if(board[coords[0]+2*diagonalCoords[i,0], coords[1]+2*diagonalCoords[i,1]]==0){
						List<int[]> captured = new List<int[]> ();
						captured.Add (new int[]{ coords [0] + 2 * diagonalCoords [i, 0], coords [1] + 2 * diagonalCoords [i, 1] });
						Move capturing = new Move(coords, new int[] {coords[0]+2*diagonalCoords[i,0], coords[1]+2*diagonalCoords[i,1], coords[2]}, captured);
						moves.Add (capturing);
					}
				}
			}
		}
		moves.AddRange (getCaptures (moves));
		return moves;
	}

	List<Move> getCaptures(List<Move> moves){
		List<Move> output = new List<Move> ();
		int opponent = 0;
		foreach (Move move in moves) {
			for (int i = 0; i<4; i++) {
				if (IsValidPosition (move.endPos [0] + diagonalCoords [i, 0], move.endPos [1] + diagonalCoords [i, 1]) && IsValidPosition (move.endPos [0] + diagonalCoords [i, 0] + diagonalCoords [i, 0], move.endPos [1] + diagonalCoords [i, 1] + diagonalCoords [i, 1])) {
					if (board [move.endPos [0] + diagonalCoords [i, 0], move.endPos [1] + diagonalCoords [i, 1]] == opponent && board [move.endPos [0] + diagonalCoords [i, 0] + diagonalCoords [i, 0], move.endPos [1] + diagonalCoords [i, 1] + diagonalCoords [i, 1]] == 0) {
						int[] newStart = move.startPos;
						int[] newEnd = new int[] { move.endPos [0] + 2 * diagonalCoords [i, 0], move.endPos [1] + 2 * diagonalCoords [i, 1] };
						int[] captured = new int[] { move.endPos [0] + diagonalCoords [i, 0], move.endPos [1] + diagonalCoords [i, 1] };
						Move toAdd = new Move (newStart, newEnd, move.capped);
						toAdd.capped.Add (captured);
						output.Add (toAdd);
					}
				}
			}
		}
		if (output.Count == 0) {
			return output;
		}
		output.AddRange (getCaptures (output));
		return output;
	}

	//Checks if the array game has been won
	bool gg (int color){
		for  (int i=0; i<8; i++){
			for (int k = 0; k<8; k++){
				if (board[i,k] == (color)||board[i,k] == (color+2))
					return false;
			}
		}
		return true;
	}

	//Makes a move on the array board
	void make (Move move){
		board [move.startPos [0], move.startPos [1]] = 0;
		board [move.endPos [0], move.endPos [1]] = move.startPos [2];
		foreach (int[] cap in move.capped) {
			board [cap [0], cap [1]] = 0;
		}
	}

	//Retracts a move on the array board
	void retract (Move move){
		board [move.startPos [0], move.startPos [1]] = move.startPos [2];
		board [move.endPos [0], move.endPos [1]] = 0;
		foreach (int[] cap in move.capped) {
			board [cap [0], cap [1]] = cap[2];
		}
	}

	//Heuristic evaluation of non-terminal game states
	//Who the fuck wrote that
	//That sounds stupid
	//It tells you who's winning
	int heval (){
		int weight1 = 1;
		int weight2 = 1;
		return (piecesLeft ()*weight1) + (distToEdge ()*weight2);
	}

	int piecesLeft(){
		int red = 0;
		int blue = 0;
		for  (int i=0; i<8; i++){
			for (int k = 0; k<8; k++){
				if (board [i,k] == (1) || board [i,k] == (3)) {
					red++;
				}
				if (board [i,k] == (2) || board [i,k] == (4)) {
					blue++;
				}
			}
		}
		return blue - red;
	}

	int distToEdge(){
		return 0;
	}

	float[] CoordToWorldPosition(int x, int y) {
		// Translates from an 8x8 grid state to an actual place in the world.
		// **IMPORTANT: Y-AXIS ON THE GRID IS Z-AXIS IN THE GAME WOLRD.**
		float xPos = 20 * x - 70;
		float yPos = 20 * y - 70;
		return new float[] {xPos, yPos};
	}

	void MakeMove (Move finalMove){
		GameObject piece = null;
		//Finds the active checker
		foreach (GameObject checker in GameObject.FindGameObjectsWithTag("GamePiece")) {
			if (checker.GetComponent<CheckerPiece> ().coords[0] == finalMove.startPos [0] && checker.GetComponent<CheckerPiece> ().coords[1] == finalMove.startPos [1])
				piece = checker;
		}

		//Moves the checker
		float [] finalPosition = CoordToWorldPosition (finalMove.endPos[0], finalMove.endPos[0]);
		piece.transform.position = new Vector3(finalPosition[0], finalPosition[1]);

		//Destroy captured pieces
		foreach (int[] cappedPiece in finalMove.capped) {
			foreach (GameObject checker in GameObject.FindGameObjectsWithTag("GamePiece")) {
				if (checker.GetComponent<CheckerPiece> ().coords[0] == cappedPiece [0] && checker.GetComponent<CheckerPiece> ().coords[1] == cappedPiece [1])
					GameObject.Destroy (checker);
			}
		}
	}

	public bool IsValidPosition(int x, int y) {
		return x >= 0 && x <= 7 && y >= 0 && y <= 7;
	}
}
