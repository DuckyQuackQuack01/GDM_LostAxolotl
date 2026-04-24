using UnityEngine;

public class MapViewToggle : MonoBehaviour
{
    [Header("Camera")]
    public Camera targetCamera;
    public CameraMovement cameraMovement;

    [Header("Player Control")]
    public SlingshotMechanic slingshotMechanic;
    public PlayerPlatformWalk platformWalk;

    [Header("Zoom Settings")]
    public float normalZoom = 5f;
    public float mapZoom = 20f;
    public float zoomSpeed = 8f;

    [Header("Optional Map Position")]
    public bool moveToMapCenter = false;
    public Vector3 mapCenterPosition = new Vector3(0f, 0f, -10f);
    public float moveSpeed = 8f;

    public bool IsMapViewActive { get; private set; }

    private Vector3 originalCameraPosition;
    private float targetZoom;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (cameraMovement == null)
            cameraMovement = GetComponent<CameraMovement>();

        originalCameraPosition = transform.position;
        targetZoom = normalZoom;

        if (targetCamera != null && targetCamera.orthographic)
            targetCamera.orthographicSize = normalZoom;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMapView();
        }

        if (targetCamera == null)
            return;

        float dt = Time.unscaledDeltaTime;

        targetCamera.orthographicSize = Mathf.Lerp(
            targetCamera.orthographicSize,
            targetZoom,
            zoomSpeed * dt
        );

        if (IsMapViewActive && moveToMapCenter)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                mapCenterPosition,
                moveSpeed * dt
            );
        }
    }

    void ToggleMapView()
    {
        IsMapViewActive = !IsMapViewActive;

        if (IsMapViewActive)
        {
            originalCameraPosition = transform.position;
            targetZoom = mapZoom;
            Time.timeScale = 0f;

            if (cameraMovement != null)
                cameraMovement.enabled = false;

            if (slingshotMechanic != null)
                slingshotMechanic.CancelInteraction();
        }
        else
        {
            targetZoom = normalZoom;
            Time.timeScale = 1f;

            if (cameraMovement != null)
                cameraMovement.enabled = true;
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        IsMapViewActive = false;

        if (cameraMovement != null)
            cameraMovement.enabled = true;
    }
}