using UnityEngine;

public class PlayerPlatformWalk : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float surfaceOffset = 0.02f;

    public Transform spriteTransform;

    public SlingshotMechanic slingshot;

    private bool canWalk = false;
    private Transform leftLimit;
    private Transform rightLimit;

    private float currentT = 0f;

    public void EnableWalk(Transform leftEdge, Transform rightEdge)
    {
        canWalk = true;
        leftLimit = leftEdge;
        rightLimit = rightEdge;

        if (leftLimit == null || rightLimit == null)
            return;

        Vector2 leftPos = leftLimit.position;
        Vector2 rightPos = rightLimit.position;
        Vector2 platformVector = rightPos - leftPos;

        float sqrLength = platformVector.sqrMagnitude;

        if (sqrLength > 0.0001f)
        {
            Vector2 playerPos = transform.position;
            currentT = Vector2.Dot(playerPos - leftPos, platformVector) / sqrLength;
            currentT = Mathf.Clamp01(currentT);
        }
        else
        {
            currentT = 0f;
        }

        AlignToPlatform();
        UpdatePlayerPosition();
    }

    public void DisableWalk()
    {
        canWalk = false;
        leftLimit = null;
        rightLimit = null;
    }

    void Update()
    {
        if (!canWalk)
            return;

        if (leftLimit == null || rightLimit == null)
            return;

        if (slingshot != null && slingshot.isDragging)
            return;

        float moveInput = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            Flip(false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            Flip(true);
        }

        Vector2 leftPos = leftLimit.position;
        Vector2 rightPos = rightLimit.position;
        float platformLength = Vector2.Distance(leftPos, rightPos);

        if (platformLength <= 0.0001f)
            return;

        float deltaT = (moveInput * moveSpeed * Time.deltaTime) / platformLength;
        currentT = Mathf.Clamp01(currentT + deltaT);

        AlignToPlatform();
        UpdatePlayerPosition();
    }

    void UpdatePlayerPosition()
    {
        if (leftLimit == null || rightLimit == null)
            return;

        Vector2 leftPos = leftLimit.position;
        Vector2 rightPos = rightLimit.position;

        Vector2 pointOnPlatform = Vector2.Lerp(leftPos, rightPos, currentT);

        Vector2 platformDir = (rightPos - leftPos).normalized;
        Vector2 platformNormal = new Vector2(-platformDir.y, platformDir.x);

        Vector2 finalPos = pointOnPlatform + platformNormal * surfaceOffset;

        transform.position = new Vector3(finalPos.x, finalPos.y, transform.position.z);
    }

    void AlignToPlatform()
    {
        Vector2 leftPos = leftLimit.position;
        Vector2 rightPos = rightLimit.position;

        Vector2 platformDir = rightPos - leftPos;

        float angle = Mathf.Atan2(platformDir.y, platformDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Flip(bool facingRight)
    {
        if (spriteTransform == null)
            return;

        Vector3 scale = spriteTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
        spriteTransform.localScale = scale;
    }
}