using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerAnimationController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 currentVelocity;
    float rotationSpeed;

    Vector2 startMousePosition;

    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.linearVelocity;
        UpdateRotation();

        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && currentVelocity == Vector2.zero)
        {
            UpdateSlingshotAnimation();
        }
    }

    void UpdateRotation()
    {
        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void UpdateSlingshotAnimation()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 directionAway = startMousePosition - mousePosition;

        float angle = Mathf.Atan2(directionAway.y, directionAway.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MouseButtonDown()
    {
        
    }
}
