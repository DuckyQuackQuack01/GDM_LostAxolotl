using UnityEngine;
using System.Collections;

public class SlowTimeEffect : MonoBehaviour
{
    public float slowMultiplier = 0.5f;

    private Coroutine activeCoroutine;

    public void Activate(float duration) // ← MUST take float
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(HandleSlowTime(duration));
    }

    private IEnumerator HandleSlowTime(float duration)
    {
        Time.timeScale = slowMultiplier;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        activeCoroutine = null;
    }
}