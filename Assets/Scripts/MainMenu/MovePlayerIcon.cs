using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayerIcon : MonoBehaviour
{
    public float moveDuration = 1.5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float timer = 0f;
    private bool isMoving = false;

    private string sceneToLoad;

    public void MoveToPointAndLoad(Vector3 destination, string sceneName)
    {
        startPosition = transform.position;
        targetPosition = destination;
        sceneToLoad = sceneName;

        LevelEntryState.playIntro = true;

        timer = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);

        float curvedT = t * t;

        transform.position = Vector3.Lerp(startPosition, targetPosition, curvedT);

        if (t >= 1f)
        {
            transform.position = targetPosition;
            isMoving = false;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}