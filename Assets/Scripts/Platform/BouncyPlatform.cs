using UnityEngine;

public class BouncyPlatform : MonoBehaviour
{
    public float bounceForce = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                Vector2 bounceDirection = (other.transform.position - transform.position).normalized;
                rb.linearVelocity = bounceDirection * bounceForce;
            }
        }
    }
}