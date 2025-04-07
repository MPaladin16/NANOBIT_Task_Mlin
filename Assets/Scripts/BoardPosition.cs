using System;
using UnityEngine;

public class BoardPosition : MonoBehaviour
{

    [SerializeField] int index;
    [SerializeField] GameObject currentPiece;

    [SerializeField] GameManager gameManager;

    private void OnMouseDown()
    {
        //Do Method depending on the GamePhase or if removing is true
        if(gameManager.IsRemoving())
        {
            //destroy piece if selected piece is for an opposite player
            if (currentPiece != null && currentPiece.GetComponent<Piece>().GetOwner() != gameManager.GetCurrentPlayer())
            {
                if (!gameManager.IsPartOfMill(index) || gameManager.AllOpponentPiecesInMills())
                {
                    Destroy(currentPiece);
                    currentPiece = null;
                    gameManager.FinishRemove();
                }
                else
                {
                    Debug.Log("Can't remove a piece that's in a mill (unless all are).");
                    Debug.Log("Choose another piece");
                }
            }
        }
        else if (gameManager.GetCurrentPhase() == GameManager.GamePhase.Placement)
        {
            gameManager.TryPlacePiece(index);
        }
        else if (gameManager.GetCurrentPhase() == GameManager.GamePhase.Movement)
        {
            gameManager.HandleMovementClick(index);
        }
    }

    //Get & Set
    public bool IsOccupied => currentPiece != null;
    public GameObject GetPiece() => currentPiece;
    public void SetPiece(GameObject go) => currentPiece = go;
    public int GetIndex() => index;


}
