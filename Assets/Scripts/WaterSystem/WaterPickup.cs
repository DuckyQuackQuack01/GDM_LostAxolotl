using UnityEngine;

public class WaterPickup : MonoBehaviour
{
    public float waterAmount = 20f;
    public GameObject pickupEffect;

    private bool collected = false;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected)
            return;

        collected = true;

        WaterManager.Instance.AddWater(waterAmount);

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        if (anim != null)
        {
            anim.Play("DropletPickup");
            Destroy(gameObject, 0.3f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ResetPickup()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}
