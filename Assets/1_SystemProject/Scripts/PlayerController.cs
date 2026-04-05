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

    public float speed = 5;
    public Vector2 movement;
    public Vector2 velocity;
    public float gravity = 1;
    public float jumpHeight = 5;
    public bool isGrounded;

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

        velocity.x = movement.x * speed;

        if (!isGrounded)
            velocity.y -= gravity * Time.deltaTime;

        transform.position += (Vector3)(velocity * Time.deltaTime);

        isGrounded = false;

        HitboxCheck();

        Vector2 newCameraPos;
        newCameraPos.y = cameraLock.position.y;
        newCameraPos.x = transform.position.x;
        cameraLock.position = newCameraPos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y += jumpHeight;
        }
    }

    public void HitboxCheck()
    {
        if (playerHitbox.bounds.Intersects(groundHitbox.bounds))
        {
            velocity.y = 0;
            isGrounded = true;

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
