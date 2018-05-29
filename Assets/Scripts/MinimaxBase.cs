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
	
	// If it's the correct turn and the AI is active, minimax is run
	void Update () {
		if (minimaxActive && manager.GetComponent<InitializeBoard>().playerTurn == 1) {
			minimax();
		}
	}

	//Easier way of referring to the corners.
	int[][] Corners = new int[4][];
//	Corners [0] = new int [] {1,1,0};
//	Corners [1] = new int [] {1,-1,0};
//	Corners [2] = new int [] {-1,1,0};
//	Corners [3] = new int [] {-1,-1,0};

	void minimax (){
		Move bestMove;
		int bestScore = -9999;
		boardToArray();
		List<Move> legal = getLegalMoves(2);
		foreach (Move move in legal) {
			//make (move);
			int score = min (0);
			if (score > bestScore) {
				bestScore = score;
				bestMove = move;
			}
			//retract (move);
		}

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
				//make (move);
				int score = max (depth+1);
				if (score<bestScore){
					bestScore=score;
				}
				//retract (move);
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
				//make (move);
				int score = min (depth+1);
				if (score>bestScore){
					bestScore=score;
				}
				//retract (move);
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
		if (board [coords [0] + 1, coords [1] + 1] == 0 || board [coords [0] - 1, coords [1] + 1] == 0) {
			//Add the move
		}
		if (coords [2] == 3 || coords [2] == 4) {
			if (board [coords [0] + 1, coords [1] - 1] == 0 || board [coords [0] - 1, coords [1] - 1] == 0) {
				//Add the move
			}
		}
		return moves;
	}

	//Gets list of captures that can be made
	List<Move> getCaptures(int[] coords){
		int color = coords [2];
		List<Move> moves = new List<Move> ();
		if (board [coords [0] + 1, coords [1] - 1] == 0 || board [coords [0] - 1, coords [1] - 1] == 0) {
			
		} 
		moves.AddRange (getCaptures (moves));
		return moves;
	}

	List<Move> getCaptures(List<Move> moves){
		List<Move> output = new List<Move> ();
		//FIX THIS
		int opponent = 0;
		foreach (Move move in moves) {
			foreach (int[] corner in Corners) {
				if (board [move.endPos [0] + corner [0], move.endPos [1] + corner [1]] == opponent && board [move.endPos [0] + corner [0] + corner [0], move.endPos [1] + corner [1] + corner [1]] == 0) {
					int[] newStart = move.startPos;
					int[] newEnd = new int[] {move.endPos [0] + 2 * corner [0], move.endPos [1] + 2 * corner [1] };
					int[] captured = new int[] {move.endPos [0] + corner [0], move.endPos [1] + corner [1] };
					Move toAdd = new Move (newStart, newEnd, move.capped);
					toAdd.capped.Add (captured);
					output.Add (toAdd);
				}
			}
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
}
