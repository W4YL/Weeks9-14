using UnityEngine;
using UnityEngine.Events;

public class SlimeBehaviour : MonoBehaviour
{
    public SpriteRenderer slimeHitbox;

    public SpriteRenderer playerSubHitbox;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;
    public PlayerController player;
    public SlimeSpawner spawner;

    public Vector2 velocity;
    public bool isGrounded = false;
    public int facingDirection = 0;
    public float jumpTimer;

    public float gravity = 18.6f;
    public float jumpPower = 1;
    public float jumpLength = 1;
    public float jumpCooldown = 1;
    public int hitPoints = 2;

    public UnityEvent takeDamage;

    public void InitiateComponents(PlayerController playerScript)
    {
        playerSubHitbox = playerScript.playerSubHitbox;
        groundHitbox = playerScript.groundHitbox;
        leftWallHitbox = playerScript.leftWallHitbox;
        rightWallHitbox = playerScript.rightWallHitbox;

        player = playerScript;

        playerScript.AddSlime(GetComponent<SlimeBehaviour>());
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

        if (playerSubHitbox.transform.position.x > transform.position.x)
        {
            facingDirection = 1;
        }
        if (playerSubHitbox.transform.position.x < transform.position.x)
        {
            facingDirection = -1;
        }

        if (hitPoints <= 0)
        {
            Death();
        }
    }

    public void TakeSlamDamage()
    {
        takeDamage.Invoke();
    }

    public void ReduceHp()
    {
        hitPoints--;
    }

    public void Death()
    {
        spawner.spawnedSlimes.Remove(gameObject);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        player.RemoveSlime(GetComponent<SlimeBehaviour>());
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
            groundHeight.y = -5.5f;
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
