using Unity.VisualScripting;
using UnityEngine;

public class GravityShiftTrigger : MonoBehaviour
{
    public Vector2 newGravity = new Vector2(0f, -9.81f);
    public Vector2 oldGravity = new Vector2(9.81f, 0f);

    public float cameraRotationAmount = -90f;
    public bool rotatePlayer = true;
    public float playerRotationAmount = -90f;

    private bool hasEntered = false;
    private bool canTrigger = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canTrigger)
        {
            canTrigger = false;

            CameraMovement cam = Camera.main.GetComponent<CameraMovement>();

            if (!hasEntered)
            {
                Physics2D.gravity = newGravity;

                if (cam != null)
                {
                    cam.RotateCamera(cameraRotationAmount);
                }

                if (rotatePlayer)
                {
                    other.transform.rotation *= Quaternion.Euler(0f, 0f, playerRotationAmount);
                }

                //hasEntered = true;
            }
            
            //else
            //{
            //    Physics2D.gravity = oldGravity;

            //    if (cam != null)
            //    {
            //        cam.RotateCamera(-cameraRotationAmount);
            //    }

            //    if (rotatePlayer)
            //    {
            //        other.transform.rotation *= Quaternion.Euler(0f, 0f, -playerRotationAmount);
            //    }

            //    hasEntered = false;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTrigger = true;
        }
    }
}
