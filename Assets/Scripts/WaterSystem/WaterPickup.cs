using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    [Header("Water Value")]
    public float waterAmount = 20f;

    [Header("Effects")]
    public GameObject pickupEffect;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        //Add water
        WaterManager.Instance.AddWater(waterAmount);



        //VFX
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        //Remove pickup
        Destroy(gameObject);
    }
}