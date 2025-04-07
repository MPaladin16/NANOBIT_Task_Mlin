using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool _gameOver = false;
    public void EndGame(int winner)
    {
        _gameOver = true;

        // You can show a win screen UI here
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsGameOverFlag() => _gameOver;
}
