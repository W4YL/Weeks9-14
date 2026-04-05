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
    public List<Transform> environmentHitboxTransform;

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

        isGrounded = false;

        HitboxCheck();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

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


                    Transform hbTr = hbSr.transform;

                    Vector2 groundHeight = transform.position;
                    groundHeight.y = hbSr.bounds.max.y + hbTr.localScale.y / 2;
                    transform.position = groundHeight;
                }

                else if (hbLeftCheck.bounds.Intersects(hbSr.bounds))
                {
                    velocity.x = 0;

                    Transform hbTr = hbSr.transform;

                    Vector2 wallRestraintL = transform.position;
                    wallRestraintL.x = hbSr.bounds.max.x + hbTr.localScale.x / 4;
                    transform.position = wallRestraintL;
                }

                else if (hbRightCheck.bounds.Intersects(hbSr.bounds))
                {
                    velocity.x = 0;

                    Transform hbTr = hbSr.transform;

                    Vector2 wallRestraintR = transform.position;
                    wallRestraintR.x = hbSr.bounds.max.x - hbTr.localScale.x / 4;
                    transform.position = wallRestraintR;
                }
            }
        }
    }
}
