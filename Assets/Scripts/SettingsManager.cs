using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    [SerializeField] TMP_InputField player1NameField;
    [SerializeField] TMP_InputField player2NameField;

    [SerializeField] Button[] btsList;

    private int _colorValueP1, _colorValueP2;

    private void Start()
    {
        player1NameField.text = "Player 1";
        player2NameField.text = "Player 2";
        _colorValueP1 = 0;
        _colorValueP2 = 1;
        settingsPanel.SetActive(false); 
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (!ValidateSettings()) return;

        PlayerPrefs.SetString("Player1Name", player1NameField.text);
        PlayerPrefs.SetString("Player2Name", player2NameField.text);

        PlayerPrefs.SetInt("Player1Color", _colorValueP1);
        PlayerPrefs.SetInt("Player2Color", _colorValueP2);

        SceneManager.LoadScene("GameScene");
    }

    private bool ValidateSettings()
    {
        if (player1NameField.text == player2NameField.text)
        {
            Debug.LogWarning("Players cannot have the same name!");
            player1NameField.text = "Player 1";
            player2NameField.text = "Player 2";
            return false;
        }

        if (_colorValueP2 == _colorValueP1)
        {
            Debug.LogWarning("Players cannot have the same color!");
            _colorValueP1 = 0;
            _colorValueP2 = 1;

            return false;
        }

        return true;
    }

    public void ChangePlayerColor(int i) {
        switch (i)
        {
            case 0: _colorValueP1 = 0; break;
            case 1: _colorValueP2 = 0; break;
            case 2: _colorValueP1 = 1; break;
            case 3: _colorValueP2 = 1; break;
            case 4: _colorValueP1 = 2; break;
            case 5: _colorValueP2 = 2; break;
            case 6: _colorValueP1 = 3; break;
            case 7: _colorValueP2 = 3; break;
            case 8: _colorValueP1 = 4; break;
            case 9: _colorValueP2 = 4; break;
        }
        ValidateSettings();
    }

}
