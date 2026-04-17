using UnityEngine;

public class SandstormDeathZone : MonoBehaviour
{
    public float pullSpeed = 10f;
    private Collider2D stormCollider;

    private void Awake()
    {
        stormCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();

            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                Vector2 direction = ((Vector2)transform.position - rb.position).normalized;

                rb.linearVelocity = direction * pullSpeed;
            }
        }
    }
}