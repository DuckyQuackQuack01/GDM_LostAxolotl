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
        if (WaterManager.Instance != null)
        {
            WaterManager.Instance.OnWaterChanged += UpdateBar;

            // NEW: force UI to match current water immediately
            UpdateBar(WaterManager.Instance.currentWater, WaterManager.Instance.maxWater);
        }
    }

    void UpdateBar(float current, float max)
    {
        currentWater = current;
        maxWater = max;

        if (maxWater <= 0f)
            maxWater = 1f;

        float fill = currentWater / maxWater;

        fillImage.fillAmount = fill;

        // preview should match fill by default
        previewImage.fillAmount = fill;
    }

    public void ShowPreview(float predictedWater)
    {
        if (maxWater <= 0f)
            return;

        float clamped = Mathf.Clamp(predictedWater, 0, maxWater);
        float previewFill = clamped / maxWater;

        // only update preview
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