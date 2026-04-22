using UnityEngine;
using System;

public class WaterManager : MonoBehaviour
{
    public static WaterManager Instance;

    [Header("Water Settings")]
    public float maxWater = 100f;
    public float currentWater;

    [Header("Drain Settings")]
    public float drainDuration = 60f; // seconds to go from full to empty

    private float drainRate;

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
        drainRate = maxWater / drainDuration;
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
            OnWaterDepleted?.Invoke();
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