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

    Vector3 startMousePosition;

    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        currentVelocity = rb.linearVelocity;

        UpdateRotation();

        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && currentVelocity == Vector2.zero)
        {
            UpdateSlingshot();
        }
        else if (currentVelocity == Vector2.zero && !animState.IsName("Idle"))
        {
            PlayIdleAnim();
        }
    }

    void UpdateRotation()
    {
        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void UpdateSlingshot()
    {
        Vector2 directionAway = startMousePosition - Input.mousePosition;
        UpdateSlingshotRotation(directionAway);
        UpdateSlingshotAnimation(directionAway);
    }
    private void OnMouseRelease()
    {
        Debug.Log("Mouse released");
    }

    private void UpdateSlingshotRotation(Vector2 directionAway)
    {

        float angle = Mathf.Atan2(directionAway.y, directionAway.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateSlingshotAnimation(Vector2 directionAway)
    {
        float percentage = directionAway.magnitude / 100;
        if (percentage > 0.99f)
        {
            percentage = 0.99f;
        }
        Debug.Log(percentage);
        anim.Play("ReadyingSlingshot", 0, percentage);
    }

    private void PlayIdleAnim()
    {
        anim.Play("Idle");
    }
    
}
