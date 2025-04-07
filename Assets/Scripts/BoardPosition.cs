using UnityEngine;

public class BoardPosition : MonoBehaviour
{

    [SerializeField] int index;
    [SerializeField] GameObject currentPiece; 
    private bool IsOccupied => currentPiece != null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
