using UnityEngine;

public class RotateTumbleweed : MonoBehaviour
{
    public float minSpeed = 40f;
    public float maxSpeed = 80f;

    private float rotationSpeed;
    
    void Start()
    {
        RandomiseStartRotation();
        RandomiseRotationSpeed();
    }

    private void RandomiseStartRotation()
    {
        float randomRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0,0,randomRotation);
    }

    private void RandomiseRotationSpeed()
    {
        rotationSpeed = Random.Range(minSpeed, maxSpeed);
    }


    void Update()
    {
        // rotates by amount every frame.
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
