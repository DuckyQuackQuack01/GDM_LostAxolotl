using UnityEngine;
using System.Collections;
public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    private Quaternion defaultRotation;

    private bool isRotating = false;
    private Quaternion targetRotation;
    void Start()
    {
        targetRotation = transform.rotation;
        defaultRotation = transform.rotation;
    }
    void LateUpdate()
    {
        FollowTarget();
        HandleRotation();
    }

    void FollowTarget()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = offset.z;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }

    void HandleRotation()
    {
        if (!isRotating) return;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    public void RotateCamera(float angle)
    {
        targetRotation = targetRotation * Quaternion.Euler(0f, 0f, angle);
        isRotating = true;
    }

    public void ResetCameraRotation()
    {
        transform.rotation = defaultRotation;
        targetRotation = defaultRotation;
        isRotating = false;
    }
}
