using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public GameObject pieceInfoPrefab;
	public Transform piecesParent;
	public Piece[,] pieces = new Piece[3, 3];
	private void Awake()
	{
		instance = this;
		for (int i = 0; i < 3; i++)
		{
			for(int j = 0; j < 3; j ++){
				if(i == 2 && j == 2) pieces[i, j] = new Piece(i, j, null);
				else {
					GameObject go = Instantiate(pieceInfoPrefab, piecesParent, true);
					pieces[i, j] = new Piece(i, j, go);
				}
			}
		}
	}
	public void SearchAvailableSlot(Piece piece){
		for(int i = 0; i < 3; i++){ 
			for(int j = 0; j < 3; j++){ 
				//if(spaces[i,j] == piece.id){
				//	//esquerda
				//	if(checkAvailability(i, j - 1)){ 
					
				//	}else if(checkAvailability(i, j + 1)){ 
					
				//	}else if (checkAvailability(i-1, j)){ 
					
				//	}else if (checkAvailability(i+1, j)){ 
					
				//	}
				//}
			}
		}
	}
	bool checkAvailability(int row, int collumn){ 
		if(row >= 0 && row <3 && collumn >=0 && collumn <3){
			return true;
			//if(pieces[spaces[row,collumn]].empty){
			//	return true;
			//}else{
			//	return false;
			//}
		}else{
			return false;
		}
	}
}
[System.Serializable]
public class Piece{
	public PieceInfo pieceInfo = null;
	public int row;
	public int column;
	public Piece(int newRow, int newColumn, GameObject pieceInfoObj){
		row = newRow;
		column = newColumn;
		if(pieceInfo != null){
			pieceInfo = pieceInfo.GetComponent<PieceInfo>();

		}
	}
}
