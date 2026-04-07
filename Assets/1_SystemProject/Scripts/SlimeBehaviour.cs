using UnityEngine;
using UnityEngine.Events;
using UnityEngine.WSA;

public class SlimeBehaviour : MonoBehaviour
{
    //Slime components
    public SpriteRenderer slimeHitbox;
    public SpriteRenderer slimeSr;
    public ParticleSystem hitParticles;
    public GameObject slimeDeathParticles;

    //External hitboxes
    public SpriteRenderer playerSubHitbox;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;

    //External scripts
    public PlayerController player;
    public SlimeSpawner spawner;

    //State checks
    public Vector2 velocity;
    public bool isGrounded = false;
    public int facingDirection = 0;
    public float jumpTimer;

    //Balancing stats
    public float gravity = 18.6f;
    public float jumpPower = 1;
    public float jumpLength = 1;
    public float jumpCooldown = 1;
    public int hitPoints = 2;

    //Unity event
    public UnityEvent takeDamage;

    //Get references from the player script through the spawner
    public void InitiateComponents(PlayerController playerScript)
    {
        //Get player and environment hitbox
        playerSubHitbox = playerScript.playerSubHitbox;
        groundHitbox = playerScript.groundHitbox;
        leftWallHitbox = playerScript.leftWallHitbox;
        rightWallHitbox = playerScript.rightWallHitbox;

        //Get player script itself
        player = playerScript;

        //Give the player script a reference of the spawned slime
        playerScript.AddSlime(GetComponent<SlimeBehaviour>());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set jump interval as publically balanced stat
        jumpTimer = jumpCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //For cleaner conditional management
        ConditionalManager();

        //Add velocity to slime position
        transform.position += (Vector3)(velocity * Time.deltaTime);

        //For the jumping logic
        JumpAction();

        //For hitbox checking
        HitboxCheck();
    }

    public void ConditionalManager()
    {
        if (!isGrounded)
        {
            //Assign gravity when not on the ground
            velocity.y -= gravity * Time.deltaTime;
        }

        //Save facing direction depending on the player position
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
            //Activates death function when HP drops to 0
            Death();
        }

        //Set slime facing direction
        if (facingDirection < 0)
        {
            slimeSr.flipX = false;
        }
        else
        {
            slimeSr.flipX = true;
        }
    }

    public void TakeSlamDamage()
    {
        //Invoke unity event when conditions from the player script are met
        takeDamage.Invoke();
    }

    public void ReduceHp()
    {
        //Unity event triggered function to reduce HP
        hitPoints--;
    }

    //On death
    public void Death()
    {
        //Instantiate a death particle prefab at the location of death
        Instantiate(slimeDeathParticles, transform.position, Quaternion.Euler(-120, 0, 0));

        //Remove list component from the spawner
        spawner.spawnedSlimes.Remove(gameObject);

        //Remove this current slime script from the player script
        player.RemoveSlime(GetComponent<SlimeBehaviour>());

        //Destroy slime prefab
        Destroy(gameObject);
    }

    public void JumpAction()
    {
        //Timer countdown
        jumpTimer -= Time.deltaTime;

        //When timer hits 0
        if (jumpTimer <= 0)
        {
            //Turn off grounded state
            isGrounded = false;

            //Assign jump velocity with slight diviations from balanced stat range
            velocity.y = jumpPower * Random.Range(0.8f, 1.2f);

            //Assign horizontal velocity based on the faced direction to jump towards the player
            velocity.x = jumpLength * facingDirection;

            //Reset timer with slight diviations from balanced stat range
            jumpTimer = jumpCooldown * Random.Range(0.8f, 1.2f);
        }
    }

    public void HitboxCheck()
    {
        //When player hitbox intersects with the ground
        if (slimeHitbox.bounds.Intersects(groundHitbox.bounds) && velocity.y <= 0)
        {
            //When on the ground
            if (!isGrounded)
            {
                //Turn on grounded state
                isGrounded = true;

                //Stop velocity completely
                velocity = Vector2.zero;
            }

            //Lock slime to ground height
            Vector2 groundHeight = transform.position;
            groundHeight.y = -5.5f;
            transform.position = groundHeight;
        }

        //When player hitbox intersects with the left wall
        if (slimeHitbox.bounds.Intersects(leftWallHitbox.bounds))
        {
            //Zero out x velocity
            velocity.x = 0;

            //Lock slime to right of the left wall
            Vector2 leftBoundary = transform.position;
            leftBoundary.x = -38.65f;
            transform.position = leftBoundary;
        }

        if (slimeHitbox.bounds.Intersects(rightWallHitbox.bounds))
        {
            //Zero out x velocity
            velocity.x = 0;

            //Lock slime to left of the right wall
            Vector2 rightBoundary = transform.position;
            rightBoundary.x = 38.65f;
            transform.position = rightBoundary;
        }
    }
}
