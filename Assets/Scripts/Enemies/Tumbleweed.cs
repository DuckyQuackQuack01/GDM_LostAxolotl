using UnityEngine;
using UnityEngine.SceneManagement;

public class Tumbleweed : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float activationRadius = 5f;

    private Rigidbody2D rb;
    private bool activated = false;
    private Vector2 moveDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        if (!activated)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance <= activationRadius)
                {
                    Activate(player.transform.position);
                }
            }
        }
        else
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    private void Activate(Vector2 playerPos)
    {
        activated = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        moveDirection = ((Vector2)playerPos - (Vector2)transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
