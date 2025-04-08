using UnityEngine;

public class Piece : MonoBehaviour
{
    private int owner; // 0 = Player 1, 1 = Player 2
    private Transform _highlight;
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = (owner == 0) ? Color.white : Color.red;
        _highlight = transform.Find("Highlight");
    }

    public void SetHighlight(bool value)
    {
        if (_highlight != null)
            _highlight.gameObject.SetActive(value);
    }

    //Get & Set
    public int GetOwner() => owner;
    public void SetOwner(int i) => owner = i;
}
