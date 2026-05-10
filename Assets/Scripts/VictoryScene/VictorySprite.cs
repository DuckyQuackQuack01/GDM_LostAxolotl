using UnityEngine;

public class VictorySprite : MonoBehaviour
{
    [Header("Intro Zoom Out")]
    public float startScale = 1.5f;
    public float endScale = 1f;
    public float introDuration = 1.2f;

    [Header("Pulse")]
    public bool enablePulse = true;      // turn OFF on background
    public float pulseSpeed = 1.2f;
    public float pulseAmount = 0.025f;

    private float timer = 0f;
    private bool introDone = false;

    void Start()
    {
        transform.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        if (!introDone)
        {
            timer += Time.deltaTime;
            float t = timer / introDuration;
            t = 1f - Mathf.Pow(1f - Mathf.Clamp01(t), 3f);

            float currentScale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = Vector3.one * currentScale;

            if (timer >= introDuration)
            {
                introDone = true;
                transform.localScale = Vector3.one * endScale;
            }
        }
        else if (enablePulse)
        {
            float pulse = 1f + ((Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f) * pulseAmount;
            transform.localScale = Vector3.one * (endScale * pulse);
        }
    }
}