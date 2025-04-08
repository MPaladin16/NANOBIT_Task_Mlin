using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSlideManager : MonoBehaviour
{
    public void RestartWithSlide()
    {
        this.gameObject.SetActive(true);
        this.gameObject.GetComponent<Animator>().SetTrigger("SlideIn");
        StartCoroutine(RestartAfterSlide());
    }

    IEnumerator RestartAfterSlide()
    {
        yield return new WaitForSecondsRealtime(0.99f); 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
