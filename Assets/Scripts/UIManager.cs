﻿using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PhaseText;
    [SerializeField] TextMeshProUGUI MoveText;
    [SerializeField] GameObject winPanel;
    [SerializeField] TextMeshProUGUI winText;

    [SerializeField] GameManager gm;

    private bool _gameOver = false;

    private void Start()
    {
        UpdateInstruction("Place a piece, 9 left");
    }
    public void OnGameEnd(object sender, WinEvent w)
    {
        //AudioManager.Instance.PlayWinSound();
        _gameOver = true;
        winPanel.SetActive(true);

        string name = PlayerPrefs.GetString(w.winner == 0 ? "Player1Name" : "Player2Name");
        int colorIndex = PlayerPrefs.GetInt(w.winner == 0 ? "Player1Color" : "Player2Color");
        
        Color playerColor = (w.winner == 0 ? gm.GetPlayer1Color() : gm.GetPlayer2Color());

        winText.text = $"{name} wins!";
        winText.color = playerColor;

        winPanel.GetComponent<Animator>().SetTrigger("Open");

        Debug.Log("WIN_UI_Canvas Opened");
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
        SceneManager.LoadScene("MainMenuScene"); 
    }
    public bool IsGameOverFlag() => _gameOver;
}
