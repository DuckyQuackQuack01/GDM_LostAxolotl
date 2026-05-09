using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerAnimationController : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 currentVelocity;

    Vector3 startMousePosition;

    public Animator anim;

    private AnimatorStateInfo animState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animState = anim.GetCurrentAnimatorStateInfo(0);
        currentVelocity = rb.linearVelocity;

        if (currentVelocity.magnitude != 0)
        {
            UpdateRotation();
        }

        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ResetRotation();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMouseRelease();
        }

        if (Input.GetMouseButton(0) && currentVelocity == Vector2.zero)
        {
            UpdateSlingshot();
        } 
    }

    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    private void ResetRotation()
    {
        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        Transform spriteTransform = transform.Find("Sprite");
        spriteTransform.localScale = Vector3.one;
    }

    private void UpdateSlingshot()
    {
        Vector3 directionAway = startMousePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateRotation();
        UpdateSlingshotRotation(directionAway);
        UpdateSlingshotAnimation(directionAway);
    }


    private void UpdateSlingshotRotation(Vector2 directionAway)
    {
        float angle = Mathf.Atan2(directionAway.y, directionAway.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateSlingshotAnimation(Vector2 directionAway)
    {
        float percentage = directionAway.magnitude;
        if (percentage > 0.99f)
        {
            percentage = 0.99f;
        }
        anim.Play("ReadyingSlingshot", 0, percentage);
    }

    private void OnMouseRelease()
    {
        anim.Play("StartingToFly");
    }

    private void PlayIdleAnim()
    {
        if (!animState.IsName("Idle")) {
            anim.Play("Idle");
        }
    }

    private void PlayWalkAnim()
    {
        if (!animState.IsName("Walking"))
        {
            anim.Play("Walking");
        }
    }

    public void updateToWalk(bool isWalking)
    {
        if (isWalking)
        {
            PlayWalkAnim();
        }
        else
        {
            PlayIdleAnim();
        }
    }
}
