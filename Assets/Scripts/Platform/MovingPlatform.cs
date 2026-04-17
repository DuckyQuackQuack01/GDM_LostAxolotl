using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    public float normalSpeed = 3f;
    public float slowSpeed = 1f;

    public float speed = 3f;

    private Vector3 target;

    void Start()
    {
        speed = normalSpeed;
        target = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    public void SlowDown()
    {
        speed = slowSpeed;
    }

    public void RestoreSpeed()
    {
        speed = normalSpeed;
    }
}

