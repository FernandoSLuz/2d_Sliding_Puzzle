using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PieceInfo : MonoBehaviour
{
	public TextMeshProUGUI pieceName;
	public Piece piece;
	public Button btn;
	public void LoadPiece(Piece actualPiece){
		piece = actualPiece;
		pieceName = GetComponentInChildren<TextMeshProUGUI>();
		pieceName.text = (((piece.row * 3) + piece.column)+1).ToString();
		btn = GetComponent<Button>();
		btn.onClick.AddListener(() => {
			GameManager.instance.SearchAvailableSlot(piece);
		});
	}
}