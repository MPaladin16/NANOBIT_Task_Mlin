using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSlideManager : MonoBehaviour
{
    public void RestartWithSlide()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Animator>().SetTrigger("Slide");
        StartCoroutine(RestartAfterSlide());
    }

    IEnumerator RestartAfterSlide()
    {
        yield return new WaitForSecondsRealtime(1f); 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
