    using UnityEngine;

public class LevelIntroManager : MonoBehaviour
{
    public GameObject dummyPlayer;
    public GameObject realPlayer;
    public Transform entryPoint;
    public Transform startPoint;

    public float moveDuration = 1.5f;

    private float timer;
    private bool isPlayingIntro;

    void Start()
    {
        if (LevelEntryState.playIntro)
        {
            StartIntro();
        }
        else
        {
            realPlayer.transform.position = startPoint.position;
            realPlayer.SetActive(true);
            dummyPlayer.SetActive(false);
        }
    }

    void StartIntro()
    {
        isPlayingIntro = true;
        timer = 0f;

        dummyPlayer.SetActive(true);
        realPlayer.SetActive(false);

        dummyPlayer.transform.position = entryPoint.position;
    }

    void Update()
    {
        if (!isPlayingIntro) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);
        float curvedT = t * t;

        dummyPlayer.transform.position =
            Vector3.Lerp(entryPoint.position, startPoint.position, curvedT);

        if (t >= 1f)
        {
            dummyPlayer.SetActive(false);

            realPlayer.transform.position = startPoint.position;
            realPlayer.SetActive(true);

            isPlayingIntro = false;
            LevelEntryState.playIntro = false;
        }
    }
}