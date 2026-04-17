using UnityEngine;
using UnityEngine.UI;

public class WaterBarUI : MonoBehaviour
{
    public Image fillImage;
    public Image previewImage;

    private float currentWater;
    private float maxWater;

    void Start()
    {
        WaterManager.Instance.OnWaterChanged += UpdateBar;
    }

    void UpdateBar(float current, float max)
    {
        currentWater = current;
        maxWater = max;

        float fill = current / max;

        fillImage.fillAmount = fill;

        // IMPORTANT: preview should match fill by default
        previewImage.fillAmount = fill;
    }

    // Call this while dragging
    public void ShowPreview(float predictedWater)
    {
        float clamped = Mathf.Clamp(predictedWater, 0, maxWater);

        float previewFill = clamped / maxWater;

        // ONLY update preview (do NOT touch fillImage here)
        previewImage.fillAmount = previewFill;
    }

    public void ResetPreview()
    {
        previewImage.fillAmount = fillImage.fillAmount;
    }

    void OnDestroy()
    {
        if (WaterManager.Instance != null)
            WaterManager.Instance.OnWaterChanged -= UpdateBar;
    }
}