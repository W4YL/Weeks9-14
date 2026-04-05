using UnityEngine;

public class SlimeBehaviour : MonoBehaviour
{
    public SpriteRenderer slimeHitbox;

    public SpriteRenderer playerHitbox;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;

    public Vector2 velocity;
    public bool isGrounded = false;
    public int facingDirection = 0;
    public float jumpTimer;

    public float gravity = 18.6f;
    public float jumpPower = 1;
    public float jumpLength = 1;
    public float jumpCooldown = 1;

    public void InitiateComponents(PlayerController playerScript)
    {
        playerHitbox = playerScript.playerHitbox;
        groundHitbox = playerScript.groundHitbox;
        leftWallHitbox = playerScript.leftWallHitbox;
        rightWallHitbox = playerScript.rightWallHitbox;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpTimer = jumpCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        ConditionalManager();

        transform.position += (Vector3)(velocity * Time.deltaTime);

        JumpAction();

        HitboxCheck();
    }

    public void ConditionalManager()
    {
        if (!isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        if (playerHitbox.transform.position.x > transform.position.x)
        {
            facingDirection = 1;
        }
        if (playerHitbox.transform.position.x < transform.position.x)
        {
            facingDirection = -1;
        }
    }

    public void JumpAction()
    {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0)
        {
            isGrounded = false;
            velocity.y = jumpPower * Random.Range(0.8f, 1.2f);
            velocity.x = jumpLength * facingDirection;
            jumpTimer = jumpCooldown;
        }
    }

    public void HitboxCheck()
    {
        //When player hitbox intersects with the ground
        if (slimeHitbox.bounds.Intersects(groundHitbox.bounds) && velocity.y <= 0)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                velocity = Vector2.zero;
            }

            //Lock player to ground height
            Vector2 groundHeight = transform.position;
            groundHeight.y = -7.5f;
            transform.position = groundHeight;
        }

        //When player hitbox intersects with the left wall
        if (slimeHitbox.bounds.Intersects(leftWallHitbox.bounds))
        {
            //Zero out velocity
            velocity.x = 0;

            //Lock player to right of the left wall
            Vector2 leftBoundary = transform.position;
            leftBoundary.x = -38.65f;
            transform.position = leftBoundary;
        }

        if (slimeHitbox.bounds.Intersects(rightWallHitbox.bounds))
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
