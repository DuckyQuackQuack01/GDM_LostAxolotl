using UnityEngine;

public class VictoryScene : MonoBehaviour
{
    public float loopStart = 0f;
    public float loopEnd = 32f;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.musicSource.Stop();
            AudioManager.Instance.PlayVictoryMusic();
            AudioManager.Instance.PlayDrinking();
        }
    }

    void Update()
    {
        if (AudioManager.Instance != null)
        {
            AudioSource music = AudioManager.Instance.musicSource;
            if (music.isPlaying && music.time >= loopEnd)
            {
                music.time = loopStart;
            }
        }
    }
}