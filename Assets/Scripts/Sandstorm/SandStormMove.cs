using UnityEngine;

public class SandStormMove : MonoBehaviour
{
    public float moveDuration = 1.5f;

    public Transform startPoint;
    public Transform targetPoint;

    private float timer = 0f;
    private bool isMoving = false;

    public void MoveToPoint(Transform destination)
    {
        startPoint = transform; // current position
        targetPoint = destination;

        timer = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Print");
            if (targetPoint != null)
                MoveToPoint(targetPoint);
        }

        if (!isMoving || startPoint == null || targetPoint == null) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);

        float curvedT = t * t; // ease-in

        transform.position = Vector3.Lerp(
            startPoint.position,
            targetPoint.position,
            curvedT
        );

        if (t >= 1f)
        {
            transform.position = targetPoint.position;
            isMoving = false;
        }
    }
}