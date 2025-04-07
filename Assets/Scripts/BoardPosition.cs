using UnityEngine;

public class BoardPosition : MonoBehaviour
{

    [SerializeField] int index;
    [SerializeField] GameObject currentPiece;

    [SerializeField] GameManager gameManager;

    private void OnMouseDown()
    {
        Debug.Log($"Clicked on position {index}");

        if(gameManager.IsRemoving())
        {
            if (currentPiece != null && currentPiece.GetComponent<Piece>().GetOwner() != gameManager.GetCurrentPlayer())
            {
                Destroy(currentPiece);
                currentPiece = null;
                gameManager.FinishRemove();
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


}
