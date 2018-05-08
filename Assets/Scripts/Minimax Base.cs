using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxBase : MonoBehaviour {
	bool minimaxActive = false;
	int[][] board;
	GameObject manager;
	
	// If it's the correct turn and the AI is active, minimax is run
	void Update () {
//		if (minimaxActive && InitializeBoard.playerTurn == 1) {
//			minimax();
//		}
	}

	void minimax (){
		int[] bestMove;
		int depth = 5;
		int bestScore = -9999;
		board = boardToArray();
		int[][] legal = getLegalMoves ();
		foreach (int[] move in legal) {
			//make (move, board);
			int score = min ();
			if (score > bestScore) {
				bestScore = score;
				bestMove = move;
			}
			//retract (move, board);
		}

	}

	//Finds best move for AI
	int min (){
		if (gg (2))
			return 1000;
//		else if(){ }
	}

	//Finds best move for Human
//	int max (){
//		if (gg (1))
//			return -1000;
//		return 0;
//	}

	//Turns the physical board into an array that can be passed through the minimax
	int[][] boardToArray () {
		int[][] gameboard = new int[8][8];
		for(int i=0; i<8; i++){
			for (int k = 0; k<8; k++){
				gameboard [i][k] = 0;
			}
		}
		foreach (GameObject checker in GameObject.FindGameObjectsWithTag("GamePiece")) {
			gameboard [checker.GetComponent<CheckerPiece>().coords [0]] [checker.GetComponent<CheckerPiece>().coords [1]] = checker.GetComponent<CheckerPiece>().state;
		}
	}

	//Gets a list of all legal moves
//	//How these moves are formatted tbd
//	int[][] getLegalMoves(){
//		
//	}

	//Checks if the array game has been won
	bool gg (int color){
		for  (int i=0; i<8; i++){
			for (int k = 0; k<8; k++){
				if (board[i][k] == (color)||board[i][k] == (color+2))
					return false;
			}
		}
		return true;
	}

	//Heuristic evaluation of non-terminal game states
	//Who the fuck wrote that
	//That sounds stupid
	//It tells you who's winning
//	int heval (int [][] board){
//		
//	}
}
