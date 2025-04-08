using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PhaseText;
    [SerializeField] TextMeshProUGUI MoveText;
    [SerializeField] GameObject winPanel;
    [SerializeField] TextMeshProUGUI winText;

    private bool _gameOver = false;

    private void Start()
    {
        UpdateInstruction("Place a piece, 9 left");
    }
    public void EndGame(int winner)
    {
        _gameOver = true;
        winPanel.SetActive(true);
        string s = (winner == 0) ? "White" : "Red";
        winText.text = $"{s} wins!";
        winText.color = (winner == 0) ? Color.white : Color.red;
        winPanel.GetComponent<Animator>().SetTrigger("Open");
    }
    public void UpdateTurnText(string playerName, Color playerColor)
    {
        PhaseText.text = playerName;
        PhaseText.color = playerColor;
    }

    public void UpdateInstruction(string text)
    {
        MoveText.text = text;
    }

    public void RestartGame() {
        winPanel.GetComponent<Animator>().SetTrigger("Close");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMainMenu()
    {
        winPanel.GetComponent<Animator>().SetTrigger("Close");
        SceneManager.LoadScene("MainMenuScene"); // your main menu scene name
    }
    public bool IsGameOverFlag() => _gameOver;
}
