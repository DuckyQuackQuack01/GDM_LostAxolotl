using UnityEngine;

public class WaterPickupResetter : MonoBehaviour
{
    public static WaterPickupResetter Instance;

    private WaterPickup[] pickups;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pickups = FindObjectsByType<WaterPickup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public void ResetAllPickups()
    {
        foreach (WaterPickup pickup in pickups)
        {
            pickup.ResetPickup();
        }
    }
}