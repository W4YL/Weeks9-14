using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer playerHitbox;
    public SpriteRenderer hbBottomCheck;
    public SpriteRenderer hbLeftCheck;
    public SpriteRenderer hbRightCheck;
    public List<SpriteRenderer> environmentHitbox;

    public Transform cameraLock;

    public float speed = 5;
    public Vector2 movement;
    public Vector2 velocity;
    public float gravity = 1;
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

        HitboxCheck();
    }

    public void HitboxCheck()
    {
        foreach (SpriteRenderer hbSr in environmentHitbox)
        {
            if (playerHitbox.bounds.Intersects(hbSr.bounds))
            {
                if (hbBottomCheck.bounds.Intersects(hbSr.bounds))
                {
                    velocity.y = 0;
                    isGrounded = true;
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
}
