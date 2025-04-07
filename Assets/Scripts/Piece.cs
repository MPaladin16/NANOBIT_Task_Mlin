using UnityEngine;

public class Piece : MonoBehaviour
{
    private int owner; // 0 = Player 1, 1 = Player 2

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = (owner == 0) ? Color.white : Color.red;
    }

    //Get & Set
    public int GetOwner() => owner;
    public void SetOwner(int i) => owner = i;
}
