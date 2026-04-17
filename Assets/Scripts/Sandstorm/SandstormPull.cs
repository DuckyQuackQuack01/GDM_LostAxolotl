using Unity.VisualScripting;
using UnityEngine;

public class SandstormGravity : MonoBehaviour
{
    public float gravityStrength = 20f;
    private Collider2D stormCollider;

    private void Awake()
    {
        stormCollider = GetComponent<Collider2D>();
    }

    public Vector2 GetForceAtPosition(Vector2 position)
    {
        if (stormCollider != null && stormCollider.OverlapPoint(position))
        {
            Vector2 direction = ((Vector2)transform.position - position).normalized;
            return direction * gravityStrength;
        }

        return Vector2.zero;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponentInParent<Rigidbody2D>();

            if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            {
                Vector2 force = GetForceAtPosition(other.transform.position);
                rb.AddForce(force);
            }
        }
    }
}