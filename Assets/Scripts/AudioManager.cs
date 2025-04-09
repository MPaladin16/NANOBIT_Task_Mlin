using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgSource;
    public AudioSource gameSource;

    [Header("Clips")]
    public AudioClip bgMusic;
    public AudioClip PieceMusic;
    public AudioClip IllegalMusic;
    public AudioClip ClickMusic;
    public AudioClip WinMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayBGM();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM()
    {
        bgSource.clip = bgMusic;
        bgSource.loop = true;
        bgSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        gameSource.PlayOneShot(clip);
    }

    public void PlayPlaceSound() => PlaySound(PieceMusic);
    public void PlayIllegalSound() => PlaySound(IllegalMusic);
    public void PlayWinSound() => PlaySound(WinMusic);
    public void PlayClickSound() => PlaySound(ClickMusic);

    public void OnGameEnd(object sender, WinEvent w) {
        PlaySound(WinMusic);
        Debug.Log("Win Music playing");
    }
}
