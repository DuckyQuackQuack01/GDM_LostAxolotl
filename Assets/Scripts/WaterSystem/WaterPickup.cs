using UnityEngine;
using System.Collections;

public class WaterPickup : MonoBehaviour
{
    public float waterAmount = 20f;
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    private bool collected = false;
    private Animator anim;
    private AudioSource audioSource;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || collected)
            return;

        collected = true;

        WaterManager.Instance.AddWater(waterAmount);

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        if (pickupSound != null && audioSource != null)
            audioSource.PlayOneShot(pickupSound);

        if (anim != null)
        {
            anim.Play("DropletPickup");
            StartCoroutine(DisableAfterAnimation());
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

