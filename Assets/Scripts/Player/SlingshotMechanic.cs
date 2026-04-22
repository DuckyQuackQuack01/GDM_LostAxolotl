using UnityEngine;
using UnityEngine.SceneManagement;

public class SlingshotMechanic : MonoBehaviour
{
    public GameObject fellPopup;

    [Header("Slingshot Settings")]
    public float maxDragDistance = 3f;
    public float launchPower = 10f;
    public float powerBoost = 1.45f; // NEW

    [Header("Water Settings")]
    public float waterCostMultiplier = 10f;
    public float maxWaterPerLaunch = 30f;
    public float minDragThreshold = 0.2f;

    [Header("Trajectory Dots")]
    public GameObject dotPrefab;
    public int dotCount = 40;
    public float dotSpacing = 0.02f;

    public WaterBarUI waterBarUI;

    private GameObject[] dots;
    private Rigidbody2D rb;

    private Vector2 startPos;
    private Vector2 originalStartPos;

    private bool isDragging = false;

    private bool onPlatform = false;
    private MovingPlatform currentPlatform;

    private Transform platformTransform;
    private Vector3 lastPlatformPosition;

    private Vector2 defaultGravity = new Vector2(0f, -9.8f);

    public CameraMovement cameraMovement;
    public SandstormGravity sandstorm;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        originalStartPos = transform.position;
        startPos = originalStartPos;

        dots = new GameObject[dotCount];
        for (int i = 0; i < dotCount; i++)
        {
            dots[i] = Instantiate(dotPrefab, startPos, Quaternion.identity);
            dots[i].SetActive(false);
        }
    }

    void OnEnable()
    {
        if (WaterManager.Instance != null)
            WaterManager.Instance.OnWaterDepleted += HandleWaterDepleted;
    }

    void OnDisable()
    {
        if (WaterManager.Instance != null)
            WaterManager.Instance.OnWaterDepleted -= HandleWaterDepleted;
    }

    void Update()
    {
        // NEW + OLD input merged
        if (Input.GetMouseButtonDown(0))
        {
            if (WaterManager.Instance.currentWater > 0)
            {
                isDragging = true;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }

        if (onPlatform && platformTransform != null)
        {
            Vector3 platformDelta = platformTransform.position - lastPlatformPosition;
            transform.position += platformDelta;

            lastPlatformPosition = platformTransform.position;
            startPos = transform.position;
        }

        if (isDragging)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragVector = mousePos - startPos;

            dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

            Vector2 launchDir = -dragVector;
            Vector2 velocity = launchDir * launchPower;

            DrawTrajectory(velocity);

            float dragDistance = dragVector.magnitude;
            float waterCost = Mathf.Min(dragDistance * waterCostMultiplier, maxWaterPerLaunch);
            float predictedWater = WaterManager.Instance.currentWater - waterCost;

            if (waterBarUI != null)
                waterBarUI.ShowPreview(predictedWater);

            if (waterBarUI != null && waterBarUI.previewImage != null)
            {
                waterBarUI.previewImage.color =
                    predictedWater < 0 ? Color.red :
                    new Color(0.5f, 0.8f, 1f, 0.6f);
            }
        }

        if (Input.GetMouseButtonUp(0))
            HandleLaunch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Physics2D.gravity = defaultGravity;
            ScoreManager.ResetScore();
        }
    }

    void HandleLaunch()
    {
        if (!isDragging) return;
        isDragging = false;

        if (waterBarUI != null)
            waterBarUI.ResetPreview();

        onPlatform = false;
        platformTransform = null;

        if (currentPlatform != null)
        {
            currentPlatform.RestoreSpeed();
            currentPlatform = null;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragVector = mousePos - startPos;

        dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

        float dragDistance = dragVector.magnitude;

        if (dragDistance < minDragThreshold)
        {
            HideDots();
            return;
        }

        float waterCost = Mathf.Min(dragDistance * waterCostMultiplier, maxWaterPerLaunch);

        if (WaterManager.Instance.currentWater < waterCost)
        {
            HideDots();
            return;
        }

        WaterManager.Instance.UseWater(waterCost);

        // NEW POWER BOOST SYSTEM
        float pullPercent = dragVector.magnitude / maxDragDistance;
        pullPercent = Mathf.Clamp01(pullPercent * powerBoost);

        Vector2 launchDir = -dragVector;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = launchDir * (launchPower * pullPercent);

        HideDots();
    }

    void DrawTrajectory(Vector2 initialVelocity)
    {
        Vector2 simPosition = startPos;
        Vector2 simVelocity = initialVelocity;
        float timeStep = dotSpacing;

        for (int i = 0; i < dotCount; i++)
        {
            Vector2 totalAcceleration = Physics2D.gravity * rb.gravityScale;

            if (sandstorm != null)
            {
                Vector2 force = sandstorm.GetForceAtPosition(simPosition);
                totalAcceleration += force / rb.mass;
            }

            simVelocity += totalAcceleration * timeStep;
            simPosition += simVelocity * timeStep;

            dots[i].transform.position = simPosition;
            dots[i].SetActive(true);
        }
    }

    void HideDots()
    {
        foreach (var dot in dots)
            dot.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            HandleFailState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlatformTop"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType = RigidbodyType2D.Kinematic;

            transform.position += new Vector3(0, 0.02f, 0);

            onPlatform = true;
            startPos = transform.position;

            ScoreManager.AddPoint();

            currentPlatform = other.GetComponentInParent<MovingPlatform>();

            if (currentPlatform != null)
            {
                currentPlatform.SlowDown();
                platformTransform = currentPlatform.transform;
                lastPlatformPosition = platformTransform.position;
            }
            else
            {
                platformTransform = other.transform;
                lastPlatformPosition = platformTransform.position;
            }

            startPos = transform.position;

            ScoreManager.AddPoint();
        }
    }

    void HandleWaterDepleted()
    {
        isDragging = false;
        HideDots();

        if (waterBarUI != null)
            waterBarUI.ResetPreview();

        HandleFailState();
    }

    void HandleFailState()
    {
        WaterManager.Instance.ResetWater();

        if (WaterPickupResetter.Instance != null)
            WaterPickupResetter.Instance.ResetAllPickups();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.position = originalStartPos;

        rb.bodyType = RigidbodyType2D.Kinematic;

        fellPopup.SetActive(true);
        Invoke(nameof(HideFellPopup), 1.5f);

        startPos = originalStartPos;
        transform.SetParent(null);

        onPlatform = false;

        if (currentPlatform != null)
        {
            currentPlatform.RestoreSpeed();
            currentPlatform = null;
        }

        cameraMovement.ResetCameraRotation();
        Physics2D.gravity = defaultGravity;

        ScoreManager.ResetScore();
        LevelEntryState.playIntro = false;
    }

    void HideFellPopup()
    {
        fellPopup.SetActive(false);
    }
}