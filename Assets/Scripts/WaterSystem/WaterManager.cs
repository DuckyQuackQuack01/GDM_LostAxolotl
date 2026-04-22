using UnityEngine;
using System;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;

    [Header("Water Settings")]
    public float maxWater = 100f;
    public float currentWater;

    [Header("Drain Settings")]
    public float drainDuration = 60f; // seconds to go from full -> empty

    private float drainRate;

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
        // how much water is lost per second
        drainRate = maxWater / drainDuration;
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
    void Update()
    {
        if (currentWater <= 0)
            return;

        float drainAmount = drainRate * Time.deltaTime;

        currentWater -= drainAmount;
        currentWater = Mathf.Clamp(currentWater, 0, maxWater);

        NotifyUI();

        if (currentWater <= 0)
        {
            OnWaterDepleted?.Invoke();
        }
    }

    // Notify UI to update bar
    void NotifyUI()
    {
        OnWaterChanged?.Invoke(currentWater, maxWater);
    }
}