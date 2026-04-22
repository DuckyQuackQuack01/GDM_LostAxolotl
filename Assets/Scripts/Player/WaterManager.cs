using UnityEngine;
using System;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;

    public float maxWater = 100f;
    public float currentWater;

    public event Action<float, float> OnWaterChanged;
    public event Action OnWaterDepleted;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentWater = maxWater;
        NotifyUI();
    }

    public void ResetWater()
    {
        currentWater = maxWater;
        NotifyUI();
    }

    public void UseWater(float amount)
    {
        currentWater -= amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        NotifyUI();

        if (currentWater <= 0)
            OnWaterDepleted?.Invoke();
    }

    public void AddWater(float amount)
    {
        currentWater += amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        NotifyUI();
    }

    void NotifyUI()
    {
        OnWaterChanged?.Invoke(currentWater, maxWater);
    }
}
