using System;
using System.Collections.Specialized;
using Unity.VisualScripting;
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
    AnimatorStateInfo animState;

    bool onPlatform = false;


    // player state Bools
    bool isFlying => currentVelocity.magnitude != 0;
    bool isReadyingSlingShot => Input.GetMouseButton(0) && currentVelocity == Vector2.zero;
    bool isIdle => currentVelocity == Vector2.zero && !animState.IsName("Idle");
    bool isWalking => onPlatform && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !animState.IsName("Walking");


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotationSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        animState = anim.GetCurrentAnimatorStateInfo(0);
        currentVelocity = rb.linearVelocity;
        
        if (isFlying)
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
        if (isReadyingSlingShot)
        {
            UpdateSlingshot();
        }
        else if (isIdle && !isWalking)
        {
            PlayIdleAnim();
            Debug.Log("Playing Idle Anim");
        }

        if (isWalking)
        {
            PlayWalkingAnim();
            Debug.Log("Trying to walk");
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
    }

    
    private void UpdateSlingshot()
    {
        Vector3 directionAway = startMousePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateSlingshotRotation(directionAway);
        UpdateSlingshotAnimation(directionAway);
    }

    // changes the players rotation to face direction of slingshot
    private void UpdateSlingshotRotation(Vector2 directionAway)
    {
        float angle = Mathf.Atan2(directionAway.y, directionAway.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // updates the slingshot animation based on how far they are pulling the slingshot
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
        onPlatform = false;
    }

    private void PlayIdleAnim()
    {
        // Checking that idle isn't already playing and player isn't walking

        anim.Play("Idle");
    }

    private void PlayWalkingAnim()
    {
        anim.Play("Walking");
    }

    // used to change onPlatform bool when player is on a platform.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlatformTop"))
        {
            onPlatform = true;
            Debug.Log("onPlatform");
        }
    }

}
