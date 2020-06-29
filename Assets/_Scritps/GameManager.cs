using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public GameObject pieceInfoPrefab;
	public Transform piecesParent;
	public Piece[,] pieces = new Piece[3, 3];
	public Vector2 origin = new Vector2();
	public GameObject waitPanel;
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
		int[] selectedPieceIndexes = new int[] { piece.actualIndexes[0], piece.actualIndexes[1] };
		if(checkAvailability(selectedPieceIndexes[0], selectedPieceIndexes[1] - 1)){
			int[] emptyPieceIndexes = new int[] { selectedPieceIndexes[0], selectedPieceIndexes[1] - 1 };
			StartCoroutine(SwapAndMoveTiles(selectedPieceIndexes, emptyPieceIndexes));
			//Debug.Log("FREE SPACE ON LEFT");
		}
		else if(checkAvailability(selectedPieceIndexes[0], selectedPieceIndexes[1] + 1)){
			int[] emptyPieceIndexes = new int[] { selectedPieceIndexes[0], selectedPieceIndexes[1] + 1 };
			StartCoroutine(SwapAndMoveTiles(selectedPieceIndexes, emptyPieceIndexes));
			//Debug.Log("FREE SPACE ON RIGHT");
		}
		else if (checkAvailability(selectedPieceIndexes[0] - 1, selectedPieceIndexes[1])){
			int[] emptyPieceIndexes = new int[] { selectedPieceIndexes[0] - 1, selectedPieceIndexes[1] };
			StartCoroutine(SwapAndMoveTiles(selectedPieceIndexes, emptyPieceIndexes));
			//Debug.Log("FREE SPACE ON TOP");
		}
		else if (checkAvailability(selectedPieceIndexes[0] + 1, selectedPieceIndexes[1])){
			int[] emptyPieceIndexes = new int[] { selectedPieceIndexes[0] + 1, selectedPieceIndexes[1] };
			StartCoroutine(SwapAndMoveTiles(selectedPieceIndexes, emptyPieceIndexes));
			//Debug.Log("FREE SPACE ON DOWN");
		}
	}
	IEnumerator SwapAndMoveTiles(int[] selectedPieceIndexes, int[] emptyPieceIndexes)
	{
		waitPanel.SetActive(true);
		Piece selectedPiece = pieces[selectedPieceIndexes[0], selectedPieceIndexes[1]];
		Piece emptyPiece = pieces[emptyPieceIndexes[0], emptyPieceIndexes[1]];

		Vector2 destinationPos = Utils.getPosition(emptyPiece.actualIndexes[1], emptyPiece.actualIndexes[0]);
		RectTransform obj = selectedPiece.pieceInfo.GetComponent<RectTransform>();
		Vector2 originPos = obj.anchoredPosition;
		float timer = 0;
		while(timer < 1.0f){
			timer += (Time.deltaTime * 1.0f) / 0.5f;
			obj.anchoredPosition = Vector2.Lerp(originPos, destinationPos, timer);
			yield return null;
		}
		selectedPiece.actualIndexes = emptyPieceIndexes;
		emptyPiece.actualIndexes = selectedPieceIndexes;
		pieces[selectedPieceIndexes[0], selectedPieceIndexes[1]] = emptyPiece;
		pieces[emptyPieceIndexes[0], emptyPieceIndexes[1]] = selectedPiece;
		if(Utils.CheckIfItIsGameOver(pieces)){
			Debug.Log("FIM DE JOGO");
		}
		else{
			//Debug.Log("FIM DE JOGO");
			waitPanel.SetActive(false);
		}
	}
	bool checkAvailability(int row, int collumn){ 
		if(row >= 0 && row <3 && collumn >=0 && collumn <3){
			if (pieces[row,collumn].pieceInfo == null) return true;
			else return false;
		}
		else return false;
	}
}
[System.Serializable]
public class Piece{
	public PieceInfo pieceInfo = null;
	public int[] originIndexes = new int[2];
	public int[] actualIndexes = new int[2];
	public Piece(int newRow, int newColumn, GameObject pieceInfoObj){
		originIndexes[0] = newRow;
		originIndexes[1] = newColumn;
		actualIndexes = originIndexes;
		if (pieceInfoObj != null){
			pieceInfo = pieceInfoObj.GetComponent<PieceInfo>();
			pieceInfo.LoadPiece(this);
		}
	}
}
