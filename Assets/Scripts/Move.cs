using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	public int state;
	public int[] startPos = new int[3];
	public int[] endPos = new int[3];
	public List<int[]> capped = new List<int[]> ();

	public Move (int[] start, int[] end){
		startPos = start;
		endPos = end;
	}

	public Move (int[] start, int[] end, List<int[]> captured){
		startPos=start;
		endPos=end;
		capped.AddRange(captured);
	}
}
