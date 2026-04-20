using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    [Header("Water Value")]
    public float waterAmount = 20f;

    [Header("Effects")]
    public GameObject pickupEffect;

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected)
            return;

        collected = true;

        WaterManager.Instance.AddWater(waterAmount);

        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
    }

    public void ResetPickup()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}