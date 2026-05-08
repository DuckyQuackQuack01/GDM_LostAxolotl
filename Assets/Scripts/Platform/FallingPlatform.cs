using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform sandstormCentre;
    public float respawnDistance = 0.5f;

    private Vector3? startPosition;

    private void Start()
    {
        Debug.Log("startPosition = " + startPosition);
        if (startPosition == null) startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, sandstormCentre.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, sandstormCentre.position) <= respawnDistance)
        {
            transform.position = startPosition ?? Vector3.zero;
        }
    }

    // Only used when using tumbleweedSpawner
    public void setNewStartPosition(Vector3 newStartPosition)
    {
        startPosition = newStartPosition;
    }
}