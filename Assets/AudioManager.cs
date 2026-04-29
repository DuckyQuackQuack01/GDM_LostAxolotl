using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource movementSource;

    [Header("Movement Clips")]
    public AudioClip jump;
    public AudioClip stick;

    [Header("Game Clips")]
    public AudioClip death;
    public AudioClip finish;

    [Header("Music")]
    public AudioClip background;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (background != null)
            PlayMusic(background);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayJump()
    {
        if (jump == null || movementSource == null) return;

        movementSource.clip = jump;
        movementSource.loop = false;
        movementSource.Play();
    }

    public void PlayStick()
    {
        if (stick == null || movementSource == null) return;

        movementSource.Stop(); // cuts jump
        movementSource.clip = stick;
        movementSource.loop = false;
        movementSource.Play();
    }

    public void PlayDeath()
    {
        if (death == null || sfxSource == null) return;

        sfxSource.PlayOneShot(death);
    }

    public void PlayFinish()
    {
        if (finish == null || sfxSource == null) return;

        sfxSource.PlayOneShot(finish);
    }
}