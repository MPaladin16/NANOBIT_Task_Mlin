using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PhaseText;
    [SerializeField] TextMeshProUGUI MoveText;
    [SerializeField] GameObject winGO;
    [SerializeField] TextMeshProUGUI winText;
    private bool _gameOver = false;

    private void Start()
    {
        UpdateTurnText(0);
        UpdateInstruction("Place a piece");
    }
    public void EndGame(int winner)
    {
        _gameOver = true;
        winGO.SetActive(true);
        string s = (winner == 0) ? "White" : "Red";
        winText.text = $"{s} wins!";
        winText.color = (winner == 0) ? Color.white : Color.red;
    }
    public void UpdateTurnText(int currentPlayer)
    {
        string color = (currentPlayer == 0) ? "White" : "Red";
        PhaseText.text = $"Player: {color}";
        PhaseText.color = (currentPlayer == 0) ? Color.white : Color.red;
    }

    public void UpdateInstruction(string text)
    {
        MoveText.text = text;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsGameOverFlag() => _gameOver;
}
