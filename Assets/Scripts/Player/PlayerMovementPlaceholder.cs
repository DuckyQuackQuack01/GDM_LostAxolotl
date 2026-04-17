using UnityEngine;

public class PlayerMovementPlaceholder : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public float groundCheckDistance = 0.6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        Vector2 gravityDir = Physics2D.gravity.normalized;
        Vector2 moveDir = new Vector2(-gravityDir.y, gravityDir.x);

        float gravityVelocity = Vector2.Dot(rb.linearVelocity, gravityDir);

        rb.linearVelocity = moveDir * moveInput * moveSpeed + gravityDir * gravityVelocity;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Vector2 gravityDir = Physics2D.gravity.normalized;
            rb.linearVelocity += -gravityDir * jumpForce;
        }
    }

    void CheckGround()
    {
        Vector2 gravityDir = Physics2D.gravity.normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            gravityDir,
            groundCheckDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        Debug.DrawRay(transform.position, gravityDir * groundCheckDistance, Color.red);
    }
}