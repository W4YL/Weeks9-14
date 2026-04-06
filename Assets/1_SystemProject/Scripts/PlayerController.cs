using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer playerSprite;

    //Sprite hitbox references
    public SpriteRenderer playerHitbox;
    public SpriteRenderer playerSubHitbox;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;
    public List<SpriteRenderer> slimeHitboxes = new List<SpriteRenderer>();

    //Horizontal camera reference point
    public Transform cameraLock;

    //State checks
    public Vector2 movement;
    public Vector2 velocity;
    public bool isGrounded = false;
    public bool canDash = false;
    public bool dashCoroutining = false;
    public bool dashJumping = false;
    public bool slideCoroutining = false;
    public bool slideJumping = false;
    public bool slamCoroutining = false;
    public bool slamJumpingCoroutine = false;
    public bool slamJumping = false;
    public bool canSlamJump = false;
    public bool movementBlock = false;
    public bool isCollidingWithSlime = false;
    public int facingDirection = 1;
    public float dashCharge = 3;

    //Balancing stats
    public float gravity = 18.6f;
    public float speed = 5;
    public float jumpHeight = 5;
    public float dashPower = 10;
    public float dashTime = 0.5f;
    public float dashCahrgingTime = 1;
    public float slidePower = 1;
    public float slamPower = 1;
    public float slamJumpWindowTime = 1;
    public float slamJumpMultiplier = 1;
    public int maxDashCharge = 3;

    public UnityEvent gotHit;
    public UnityEvent slamDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //For cleaner conditional management
        ConditionalManager();

        //For moving the camera reference point
        CameraReference();

        //For recharging dashess
        DashChargeSystem();

        //Add velocity to player position
        transform.position += (Vector3)(velocity * Time.deltaTime);

        SlimeCollision();

        //For hitbox checking
        HitboxCheck();
    }

    public void ConditionalManager()
    {
        movementBlock = !dashCoroutining && !dashJumping && !slideCoroutining && !slideJumping && !slamCoroutining;

        //Give player velocity when not performing special actions
        if (movementBlock)
        {
            velocity.x = movement.x * speed;
        }

        //Let player be affected by gravity when not on the ground + not slamming
        if (!isGrounded && !slamCoroutining)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        //Save dashing velocity when dash jumping (unless inturrupted by slam
        if (dashJumping && !slamCoroutining)
        {
            velocity.x = facingDirection * dashPower;
        }

        if (slideJumping && !slamCoroutining)
        {
            velocity.x = facingDirection * slidePower;
        }

        //Save facing direction depending on the last movement direction + when not performing special action
        if (movement.x > 0 && movementBlock)
        {
            facingDirection = 1;
        }
        else if (movement.x < 0 && movementBlock)
        {
            facingDirection = -1;
        }
    }

    public void AddSlimeHitbox(SpriteRenderer slimeHb)
    {
        slimeHitboxes.Add(slimeHb);
    }

    public void RemoveSlimeHitbox(SpriteRenderer slimeHb)
    {
        slimeHitboxes.Remove(slimeHb);
    }

    public void SlimeCollision()
    {
        foreach (SpriteRenderer slimeHb in slimeHitboxes)
        {
            if (slimeHb.bounds.Intersects(playerSubHitbox.bounds))
            {
                if (isCollidingWithSlime)
                {
                    //Is colliding
                }
                else
                {
                    gotHit.Invoke();
                }

                isCollidingWithSlime = true;
            }
            else
            {
                isCollidingWithSlime = false;
            }
        }
    }

    public void wasHit()
    {
        Debug.Log("Was hit!");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Assign movement value on input
        movement = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            if (canSlamJump)
            {
                velocity.y = jumpHeight * slamJumpMultiplier;
            }
            else
            {
                //Give jump velocity on initial button press + when the player is on the ground
                velocity.y = jumpHeight;
            }

            if (dashCoroutining && dashCharge >= 1f)
            {
                //Enable dash jumping if input performed during dash duration
                dashJumping = true;

                //Consume one extra dash charge
                dashCharge -= 1f;
            }

            if (slideCoroutining)
            {
                slideJumping = true;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash && !dashCoroutining)
        {
            //Start dash coroutine when there's charges left + if player isn't currently dashing
            StartCoroutine(DashAction());
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            slideCoroutining = true;
            StartCoroutine(SlideAction());
        }
        else if (context.started && !isGrounded)
        {
            slamCoroutining = true;
            StartCoroutine(SlamAction());
        }

        if (context.canceled)
        {
            slideCoroutining = false;
            slamCoroutining = false;
        }
    }

    IEnumerator DashAction()
    {
        //Enable dash state
        dashCoroutining = true;

        //Assign dash duration
        float dashTimer = dashTime;

        //Loop starts
        while (dashTimer > 0)
        {
            //Dash timer countdown
            dashTimer -= Time.deltaTime;

            //Assign constant velocity based on the facing direction
            velocity.x = facingDirection * dashPower;

            if (!dashJumping)
            {
                //Disable y velocity unless a dash jump is being performed
                velocity.y = 0;
            }

            yield return null;
        }

        //Consume one dash charge
        dashCharge -= 1f;

        //Disable dash state
        dashCoroutining = false;
    }

    IEnumerator SlideAction()
    {
        while (slideCoroutining)
        {
            velocity.x = facingDirection * slidePower;

            yield return null;
        }
    }

    IEnumerator SlamAction()
    {
        while (slamCoroutining)
        {
            velocity.y = -slamPower;
            velocity.x = 0;

            yield return null;
        }
    }

    IEnumerator SlamJumpWindow()
    {
        float slamJumpTimer = slamJumpWindowTime;

        while (slamJumpTimer > 0)
        {
            slamJumpTimer -= Time.deltaTime;

            canSlamJump = true;

            yield return null;
        }

        canSlamJump = false;
    }

    public void DashChargeSystem()
    {
        if (dashCharge < maxDashCharge && !dashCoroutining)
        {
            //Recharges dash when below maximum charges + isn't dashing
            dashCharge += dashCahrgingTime * Time.deltaTime;
        }
        else if (dashCharge >= maxDashCharge)
        {
            //Locks value at max charges in case of overflow
            dashCharge = maxDashCharge;
        }

        if (dashCharge >= 1)
        {
            //Enable dash when player has more than one charge
            canDash = true;
        }
        else if (dashCharge < 1)
        {
            //Disable dash when below
            canDash = false;
        }
    }

    public void CameraReference()
    {
        //Camera reference object transform mirrors the player x position
        Vector2 newCameraPos;
        newCameraPos.y = cameraLock.position.y;
        newCameraPos.x = transform.position.x;
        cameraLock.position = newCameraPos;
    }

    public void HitboxCheck()
    {
        //When player hitbox intersects with the ground
        if (playerHitbox.bounds.Intersects(groundHitbox.bounds))
        {
            //Zero out velocity
            velocity.y = 0;

            //Enable grounded state
            isGrounded = true;

            //Stop slam + dash/slide jumping velocity
            dashJumping = false;
            slideJumping = false;

            if (slamCoroutining)
            {
                slamCoroutining = false;
                StartCoroutine(SlamJumpWindow());
            }

            //Lock player to ground height
            Vector2 groundHeight = transform.position;
            groundHeight.y = -7.5f;
            transform.position = groundHeight;
        }
        else
        {
            isGrounded = false;
        }

        //When player hitbox intersects with the left wall
        if (playerHitbox.bounds.Intersects(leftWallHitbox.bounds))
        {
            //Zero out velocity
            velocity.x = 0;

            //Lock player to right of the left wall
            Vector2 leftBoundary = transform.position;
            leftBoundary.x = -38.65f;
            transform.position = leftBoundary;
        }

        if (playerHitbox.bounds.Intersects(rightWallHitbox.bounds))
        {
            //Zero out velocity
            velocity.x = 0;

            //Lock player to left of the right wall
            Vector2 rightBoundary = transform.position;
            rightBoundary.x = 38.65f;
            transform.position = rightBoundary;
        }
    }
}
