using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxBase : MonoBehaviour {
	//All you have to do to activate the AI is change this boool
	public bool minimaxActive = false;

	//Creates the array board used for the tree
	//Just like some family trees, this tree has slaves and masters
	int[,] board = new int[8,8];

	//Sets the max depth of the tree
	private int maxDepth = 5;

	public GameObject manager;
	
	// If it's the correct turn and the AI is active, minimax is run
	void Update () {
//		if (minimaxActive && InitializeBoard.playerTurn == 1) {
//			minimax();
//		}
	}

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
	//How these moves are formatted tbd
	List<Move> getLegalMoves(int color){
		List<int[]> coords = getPieceCoords (color);
		List<Move> moves = new List<Move> ();
		foreach (int[] piece in coords) {
			moves.AddRange(getAdjacentMoves (piece));
			moves.AddRange(getCaptures (piece));
		}
		return moves;
	}

	//THIS METHOD WILL NOT WORK PROPERLY YET
	//I mean it might work
	//I'm just 90% sure that it won't
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

	//Is this method recursive? I don't know yet
	//Maybe
	//But how?
	//idk
	List<Move> getCaptures(int[] coords){
		int color = coords [2];
		List<Move> moves = new List<Move> ();
		moves.Add (new Move ());
		if (board [coords [0] + 1, coords [1] - 1] == 0 || board [coords [0] - 1, coords [1] - 1] == 0) {
			
		} 
		return moves.AddRange(getCaptures(moves));
	}

	List<Move> getCaptures(List<Move> moves){
		List<Move> output = new List<Move> ();
		output.Add (new Move ());
		if (moves.Count == 0) {
			return output;
		}
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
