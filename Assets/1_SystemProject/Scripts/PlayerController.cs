using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer playerHitbox;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;

    public Transform cameraLock;


    public Vector2 movement;
    public Vector2 velocity;

    public float speed = 5;
    public float gravity = 18.6f;
    public float jumpHeight = 5;
    public float dashPower = 10;
    public float dashTime = 0.5f;
    public int facingDirection = 1;

    public bool isGrounded = false;
    public bool canDash = false;
    bool dashCoroutining = false;
    public bool dashJumping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 newPosition = transform.position;
        //newPosition.x += movement.x * speed * Time.deltaTime;
        //transform.position = newPosition;

        if (!dashCoroutining)
        {
            velocity.x = movement.x * speed;
        }

        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        if (dashJumping)
        {
            velocity.x = facingDirection * dashPower;
        }

        transform.position += (Vector3)(velocity * Time.deltaTime);

        isGrounded = false;

        if (movement.x > 0)
        {
            facingDirection = 1;
        }
        else if (movement.x < 0)
        {
            facingDirection = -1;
        }

        HitboxCheck();

        Vector2 newCameraPos;
        newCameraPos.y = cameraLock.position.y;
        newCameraPos.x = transform.position.x;
        cameraLock.position = newCameraPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!dashCoroutining)
        {
            movement = context.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            velocity.y += jumpHeight;

            if (dashCoroutining)
            {
                dashJumping = true;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {

        if (context.started && !dashCoroutining)
        {
            //Debug.Log("Dash!");

            StartCoroutine(DashAction());
        }
    }

    IEnumerator DashAction()
    {
        dashCoroutining = true;
        float dashTimer = dashTime;

        while (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
            velocity.x = facingDirection * dashPower;

            if (!dashJumping)
            {
                velocity.y = 0;
            }

            yield return null;
        }

        dashCoroutining = false;
    }

    public void HitboxCheck()
    {
        if (playerHitbox.bounds.Intersects(groundHitbox.bounds))
        {
            velocity.y = 0;
            isGrounded = true;
            dashJumping = false;

            Vector2 groundHeight = transform.position;
            groundHeight.y = -7.5f;

            transform.position = groundHeight;
        }

        if (playerHitbox.bounds.Intersects(leftWallHitbox.bounds))
        {
            velocity.x = 0;

            Vector2 leftBoundary = transform.position;
            leftBoundary.x = -38.65f;

            transform.position = leftBoundary;
        }

        if (playerHitbox.bounds.Intersects(rightWallHitbox.bounds))
        {
            velocity.x = 0;

            Vector2 rightBoundary = transform.position;
            rightBoundary.x = 38.65f;

            transform.position = rightBoundary;
        }
    }
}
