using Unity.VisualScripting;
using UnityEngine;

public class GravityShiftPlatform : MonoBehaviour
{
    public Vector2 newGravity = new Vector2(0f, -9.81f);
    public Vector2 oldGravity = new Vector2(9.81f, 0f);

    public float cameraRotationAmount = -90f;
    public bool rotatePlayer = true;
    public float playerRotationAmount = -90f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Physics2D.gravity != newGravity)
        {
            CameraMovement cam = Camera.main.GetComponent<CameraMovement>();

                Physics2D.gravity = newGravity;

                if (cam != null)
                {
                    cam.RotateCamera(cameraRotationAmount);
                }

                if (rotatePlayer)
                {
                    other.transform.rotation *= Quaternion.Euler(0f, 0f, playerRotationAmount);
                }
        }
    }
}
