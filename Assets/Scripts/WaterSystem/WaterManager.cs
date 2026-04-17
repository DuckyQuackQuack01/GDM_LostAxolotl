using UnityEngine;
using System;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;

    [Header("Water Settings")]
    public float maxWater = 100f;
    public float currentWater;

    // UI update event
    public event Action<float, float> OnWaterChanged;

    // Depletion event (used by SlingshotMechanic)
    public event Action OnWaterDepleted;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

    // Use water (called on launch)
    public void UseWater(float amount)
    {
        currentWater -= amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        NotifyUI();

        if (currentWater <= 0)
        {
            OnWaterDepleted?.Invoke();
        }
    }

    // Add water (called by pickups)
    public void AddWater(float amount)
    {
        currentWater += amount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        NotifyUI();
    }

    // Notify UI to update bar
    void NotifyUI()
    {
        OnWaterChanged?.Invoke(currentWater, maxWater);
    }
}