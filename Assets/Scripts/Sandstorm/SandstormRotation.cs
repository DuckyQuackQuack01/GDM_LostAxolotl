using UnityEngine;

public class SandstormRotation : MonoBehaviour
{

    public float rotationSpeed = 200f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
