using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public GameObject pieceInfoPrefab;
	public Transform piecesParent;
	public Piece[,] pieces = new Piece[3, 3];
	public GameObject waitPanel;
	public int moves = 0;
	public TextMeshProUGUI movesText;
	private IEnumerator Start()
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
		yield return new WaitForSeconds(0.25f);
		waitPanel.SetActive(true);
		yield return StartCoroutine(SlideTilesAnimation(30));
		waitPanel.SetActive(false);
	}
	public IEnumerator SlideTilesAnimation(int numberOfShuffles)
	{
		List<List<int[]>> availableMoves = new List<List<int[]>>();
		int[] lastPieceIndexes = new int[2] { -1, -1 };
		for(int s = 0; s < numberOfShuffles; s++){ 
			
			for(int i = 0; i < 3; i ++){ 
				for(int j = 0; j < 3; j ++){
					if(lastPieceIndexes[0] != pieces[i, j].originIndexes[0] || lastPieceIndexes[1] != pieces[i, j].originIndexes[1]){
						int[] emptyIndexes = SearchAvailableSlot(pieces[i, j]);
						if (emptyIndexes != null)
						{
							var move = new List<int[]>
						{
							pieces[i, j].actualIndexes,
							emptyIndexes
						};
							availableMoves.Add(move);
						}
					}
				}
			}

			System.Random rnd = new System.Random();
			List<int[]> myMove = availableMoves[rnd.Next(0, availableMoves.Count)];
			lastPieceIndexes = pieces[myMove[0][0], myMove[0][1]].originIndexes;
			yield return StartCoroutine(SwapAndMoveTiles(myMove[0], myMove[1], 0.10f, true));
			availableMoves.Clear();
		}
	}
	public int[] SearchAvailableSlot(Piece piece){
		
		if(checkAvailability(piece.actualIndexes[0], piece.actualIndexes[1] - 1)){
			return new int[] { piece.actualIndexes[0], piece.actualIndexes[1] - 1 };
		}
		else if(checkAvailability(piece.actualIndexes[0], piece.actualIndexes[1] + 1)){
			return new int[] { piece.actualIndexes[0], piece.actualIndexes[1] + 1 };
		}
		else if (checkAvailability(piece.actualIndexes[0] - 1, piece.actualIndexes[1])){
			return new int[] { piece.actualIndexes[0] - 1, piece.actualIndexes[1] };
		}
		else if (checkAvailability(piece.actualIndexes[0] + 1, piece.actualIndexes[1])){
			return new int[] { piece.actualIndexes[0] + 1, piece.actualIndexes[1] };
		}else{
			return null;
		}
	}
	public IEnumerator SwapAndMoveTiles(int[] selectedPieceIndexes, int[] emptyPieceIndexes, float speed = 0.25f, bool shuffle = false)
	{
		if(!shuffle) waitPanel.SetActive(true);
		Piece selectedPiece = pieces[selectedPieceIndexes[0], selectedPieceIndexes[1]];
		Piece emptyPiece = pieces[emptyPieceIndexes[0], emptyPieceIndexes[1]];

		Vector2 destinationPos = Utils.getPosition(emptyPiece.actualIndexes[1], emptyPiece.actualIndexes[0]);
		RectTransform obj = selectedPiece.pieceInfo.GetComponent<RectTransform>();
		Vector2 originPos = obj.anchoredPosition;
		float timer = 0;
		while(timer < 1.0f){
			timer += (Time.deltaTime * 1.0f) / speed;
			obj.anchoredPosition = Vector2.Lerp(originPos, destinationPos, timer);
			yield return null;
		}
		selectedPiece.actualIndexes = emptyPieceIndexes;
		emptyPiece.actualIndexes = selectedPieceIndexes;
		pieces[selectedPieceIndexes[0], selectedPieceIndexes[1]] = emptyPiece;
		pieces[emptyPieceIndexes[0], emptyPieceIndexes[1]] = selectedPiece;
		if(!shuffle){
			if (Utils.CheckIfItIsGameOver(pieces))
			{
				movesText.transform.parent.gameObject.SetActive(true);
				movesText.text = "Movimentos: " + moves.ToString();
			}
			else
			{
				waitPanel.SetActive(false);
			}
		}
	}
	public void ResetGame()
	{
		SceneManager.LoadScene(0);
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
