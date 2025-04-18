using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private int owner; // 0 = Player 1, 1 = Player 2
    private Transform _highlight;
    void Start()
    {

        GameManager gm = FindFirstObjectByType<GameManager>();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = (owner == 0) ? gm.GetPlayer1Color() : gm.GetPlayer2Color();
        _highlight = transform.Find("Highlight");
    }

    public void SetHighlight(bool value)
    {
        if (_highlight != null)
            _highlight.gameObject.SetActive(value);
    }

    public IEnumerator MoveTo(Vector3 targetPosition, float duration = 0.25f)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    //Get & Set
    public int GetOwner() => owner;
    public void SetOwner(int i) => owner = i;
}
