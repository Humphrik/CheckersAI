using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxBase : MonoBehaviour {
	bool minimaxActive = false;
	int[][] board;
	int maxDepth = 5;
	GameObject manager;
	
	// If it's the correct turn and the AI is active, minimax is run
	void Update () {
//		if (minimaxActive && InitializeBoard.playerTurn == 1) {
//			minimax();
//		}
	}

	void minimax (){
		int[] bestMove;
		int bestScore = -9999;
		board = boardToArray();
		int[][] legal = getLegalMoves ();
		foreach (int[] move in legal) {
			//make (move, board);
			int score = min (0);
			if (score > bestScore) {
				bestScore = score;
				bestMove = move;
			}
			//retract (move, board);
		}

	}

	//Finds best move for AI
	int min (int depth){
		if (gg (2))
			return 1000;
		else if (depth = maxDepth) {
			return heval ();
		} else {
			int bestScore = 9999;
			Move[] legal = getLegalMoves();
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
		else if (depth = maxDepth) {
			return heval ();
		} else {
			int bestScore = -9999;
			Move[] legal = getLegalMoves();
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
	//How these moves are formatted tbd
	Move[] getLegalMoves(){
		
	}

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

	void make (Move move){
		
	}

	void retract (Move move){
		
	}

	//Heuristic evaluation of non-terminal game states
	//Who the fuck wrote that
	//That sounds stupid
	//It tells you who's winning
	int heval (){
		
	}
}
