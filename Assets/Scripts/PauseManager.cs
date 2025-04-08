using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject ESCMenu;
    [SerializeField] Animator animator;

    [SerializeField] UIManager uiManager;
    private bool isPaused = false;

    private void Start()
    {
        ESCMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !uiManager.IsGameOverFlag())
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                ESCMenu.SetActive(true);
                animator.SetTrigger("Open");
                //Time.timeScale = 0f;
            }
            else
            {
                animator.SetTrigger("Close");
                StartCoroutine(DisableAfterClose()); // disables after making it small
                Time.timeScale = 1f;
            }
        }
    }

    IEnumerator DisableAfterClose()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        ESCMenu.SetActive(false);
    }


    public void ContinueGame()
    {
        animator.SetTrigger("Close");
        StartCoroutine(DisableAfterClose()); // disables after making it small
        Time.timeScale = 1f;
        isPaused = !isPaused;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
