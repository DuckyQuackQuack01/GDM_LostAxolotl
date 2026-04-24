using UnityEngine;
using UnityEngine.UI;

public class WaterBarUI : MonoBehaviour
{
    public Image fillImage;
    public Image previewImage;

    public float previewVisibleAlpha = 0.7f;
    public float previewHiddenAlpha = 0f;

    private float currentWater;
    private float maxWater;

    private bool showingPreview = false;
    private float previewCost = 0f;

    private RectTransform fillRect;
    private RectTransform previewRect;

    void Start()
    {
        fillRect = fillImage.rectTransform;
        previewRect = previewImage.rectTransform;

        if (WaterManager.Instance != null)
        {
            WaterManager.Instance.OnWaterChanged += UpdateBar;
            UpdateBar(WaterManager.Instance.currentWater, WaterManager.Instance.maxWater);
        }

        HidePreview();
    }

    void UpdateBar(float current, float max)
    {
        currentWater = current;
        maxWater = max <= 0f ? 1f : max;

        fillImage.fillAmount = currentWater / maxWater;

        if (showingPreview)
            RefreshPreview();
        else
            HidePreview();
    }

    public void ShowPreviewCost(float cost)
    {
        previewCost = Mathf.Max(0f, cost);
        showingPreview = true;
        RefreshPreview();
    }

    public void ResetPreview()
    {
        showingPreview = false;
        previewCost = 0f;
        HidePreview();
    }

    void RefreshPreview()
    {
        if (fillRect == null || previewRect == null)
            return;

        float totalBarWidth = fillRect.rect.width;
        float currentFillWidth = totalBarWidth * (currentWater / maxWater);

        float clampedCost = Mathf.Min(previewCost, currentWater);
        float previewWidth = totalBarWidth * (clampedCost / maxWater);
        previewWidth = Mathf.Min(previewWidth, currentFillWidth);

        previewRect.anchorMin = fillRect.anchorMin;
        previewRect.anchorMax = fillRect.anchorMax;
        previewRect.pivot = new Vector2(1f, 0.5f);
        previewRect.sizeDelta = new Vector2(previewWidth, fillRect.rect.height);

        float blueLeft = fillRect.anchoredPosition.x - (fillRect.rect.width * fillRect.pivot.x);

        float currentBlueRight = blueLeft + currentFillWidth;

        previewRect.anchoredPosition = new Vector2(currentBlueRight, fillRect.anchoredPosition.y);

        SetPreviewAlpha(previewVisibleAlpha);
    }

    void HidePreview()
    {
        if (previewRect != null)
        {
            previewRect.anchorMin = new Vector2(0f, 0f);
            previewRect.anchorMax = new Vector2(0f, 1f);
            previewRect.pivot = new Vector2(0f, 0.5f);
            previewRect.anchoredPosition = Vector2.zero;
            previewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
            previewRect.offsetMin = new Vector2(previewRect.offsetMin.x, 0f);
            previewRect.offsetMax = new Vector2(previewRect.offsetMax.x, 0f);
        }

        SetPreviewAlpha(previewHiddenAlpha);
    }

    void SetPreviewAlpha(float alpha)
    {
        if (previewImage == null)
            return;

        Color c = previewImage.color;
        c.a = alpha;
        previewImage.color = c;
    }

    void OnDestroy()
    {
        if (WaterManager.Instance != null)
            WaterManager.Instance.OnWaterChanged -= UpdateBar;
    }
}