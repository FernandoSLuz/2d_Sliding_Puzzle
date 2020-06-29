using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
	public static Vector2 getPosition(int column, int row){

		return new Vector2(-333 + (column * 333), 333 - (row * 333));
	}
	public static bool CheckIfItIsGameOver(Piece[,] pieces){
		for(int i = 0; i < 3; i++){ 
			for(int j = 0; j < 3; j ++){
				//Debug.Log("Actual indexes = " + i + " , " + j + "   ---   " + "Piece actual indexes = " + pieces[i, j].actualIndexes[0] + " , " + pieces[i, j].actualIndexes[1]);
				if (pieces[i,j].originIndexes[0] != i || pieces[i, j].originIndexes[1] != j)
				{
					return false;
				}
			}
		}
		return true;
	}
}
