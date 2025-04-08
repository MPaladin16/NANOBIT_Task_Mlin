using System;
using UnityEngine;

public class BoardPosition : MonoBehaviour
{

    [SerializeField] int index;
    [SerializeField] GameObject currentPiece;

    [SerializeField] GameManager gameManager;
    private Transform _hColor;
    private void Start()
    {
        _hColor = transform.GetChild(0).gameObject.transform;
    }
    private void OnMouseDown()
    {
        if (gameManager.IsGameOverFlag()) return;

        //Do Method depending on the GamePhase or if removing is true
        if (gameManager.IsRemoving())
        {
            //destroy piece if selected piece is for an opposite player
            if (currentPiece != null && currentPiece.GetComponent<Piece>().GetOwner() != gameManager.GetCurrentPlayer())
            {
                if (!gameManager.IsPartOfMill(index) || gameManager.AllOpponentPiecesInMills())
                {
                    AudioManager.Instance.PlayClickSound();
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
        else if (gameManager.GetCurrentPhase() == GameManager.GamePhase.Flying)
        {
            gameManager.HandleMovementClick(index); 
        }
    }

    public void SetHighlight(Color color, bool value)
    {
        if (_hColor != null)
        {
            Color c = color;
            c.a = 0.4f;
            _hColor.GetComponent<SpriteRenderer>().color = c;
            _hColor.gameObject.SetActive(value);
        }
    }

    //Get & Set
    public bool IsOccupied => currentPiece != null;
    public GameObject GetPiece() => currentPiece;
    public void SetPiece(GameObject go) => currentPiece = go;
    public int GetIndex() => index;

}
