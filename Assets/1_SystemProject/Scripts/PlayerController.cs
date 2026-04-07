using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Sprite hitbox references
    public SpriteRenderer playerHitbox;
    public SpriteRenderer playerSubHitbox;
    public SpriteRenderer playerSlamRange;
    public SpriteRenderer groundHitbox;
    public SpriteRenderer leftWallHitbox;
    public SpriteRenderer rightWallHitbox;
    public List<SlimeBehaviour> slimes = new List<SlimeBehaviour>();

    //Horizontal camera reference point
    public Transform cameraLock;

    //Visual effect references
    public Transform particleTransform;
    public ParticleManager playerParticleScript;
    bool slideParticlesPlayed = false;
    public CinemachineImpulseSource groundImpactShake;
    public CinemachineImpulseSource highImpactShake;
    public CinemachineImpulseSource dashShake;
    public CinemachineImpulseSource hitShake;
    public GameObject screenFlash;

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
    public bool highSlam = false;
    public bool exitHitstopState = true;
    public bool iFramesActive = false;
    public int facingDirection = 1;
    public float dashCharge = 3;

    //Balancing stats
    public float gravity = 18.6f;
    public float speed = 5;
    public float jumpHeight = 5;
    public float dashPower = 10;
    public float dashTime = 0.5f;
    public float dashChargingTime = 1;
    public float slidePower = 1;
    public float slamPower = 1;
    public float slamJumpWindowTime = 1;
    public float slamJumpMultiplier = 1;
    public float hitstopDuration = .1f;
    public float knockBackStrength = 2;
    public float iFrameDuration = 1;
    public int maxDashCharge = 3;

    //Unity event
    public UnityEvent gotHit;

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

        //For checking slime-player collision
        SlimeCollision();

        //For hitbox checking
        HitboxCheck();

    }

    public void ConditionalManager()
    {
        //A clean bool for blocking movement while performing special actions
        movementBlock = !dashCoroutining && !dashJumping && !slideCoroutining && !slideJumping && !slamCoroutining;

        if (movementBlock)
        {
            //Give player velocity when not performing special actions
            velocity.x = movement.x * speed;
        }

        if (!isGrounded && !slamCoroutining)
        {
            //Let player be affected by gravity when not on the ground + not slamming
            velocity.y -= gravity * Time.deltaTime;
        }

        if (dashJumping && !slamCoroutining)
        {
            //Save dashing velocity when dash jumping (unless inturrupted by slam)
            velocity.x = facingDirection * dashPower;
        }

        if (slideJumping && !slamCoroutining)
        {
            //Save sliding velocity when slide jumping (unless interrupted by slam)
            velocity.x = facingDirection * slidePower;
        }

        //Edge conditional for initially hitting the ground while sliding
        if (slideCoroutining && isGrounded)
        {
            if (slideParticlesPlayed)
            {
                //Is currently sliding
            }
            else
            {
                //Start playing the slide particle loop
                playerParticleScript.PlaySlideParticles();
            }

            //Is currently sliding on ground (currently playing particles)
            slideParticlesPlayed = true;
        }
        else
        {
            if (slideParticlesPlayed)
            {
                //Stop playing particles when first stopping the ground sliding state
                playerParticleScript.StopSlideParticles();
            }

            //Is currently not sliding on ground (no particles)
            slideParticlesPlayed = false;
        }

        //Save facing direction depending on the last movement direction + when not performing special action
        if (movement.x > 0 && movementBlock)
        {
            facingDirection = 1;

            //Unflip particle system parent when facing right
            particleTransform.localScale = new Vector2(1, 1);

            //Set dash shake direction
            dashShake.DefaultVelocity.x = 0.08f;
        }
        else if (movement.x < 0 && movementBlock)
        {
            facingDirection = -1;

            //Flip particle system parent when facing left
            particleTransform.localScale = new Vector2(-1, 1);

            //Set dash shake direction
            dashShake.DefaultVelocity.x = -0.08f;
        }
    }

    public void AddSlime(SlimeBehaviour slime)
    {
        //Add spawned slime script
        slimes.Add(slime);
    }

    public void RemoveSlime(SlimeBehaviour slime)
    {
        //Remove destroyed slime script
        slimes.Remove(slime);
    }

    public void SlimeCollision()
    {
        foreach (SlimeBehaviour slime in slimes)
        {
            //Check if any slime are colliding with player
            if (slime.slimeHitbox.bounds.Intersects(playerSubHitbox.bounds) && !iFramesActive)
            {
                if (isCollidingWithSlime)
                {
                    //Is colliding
                }
                else
                {
                    //Invoke got hit event on initial contact
                    gotHit.Invoke();
                }

                //Is currently colliding with slime
                isCollidingWithSlime = true;
            }
            else
            {
                //Is not currently colliding with slime
                isCollidingWithSlime = false;
            }
        }
    }

    public void DoSlamDamage()
    {
        foreach (SlimeBehaviour slime in slimes)
        {
            //If any slimes collides with player's slam range during slam impact check
            if (slime.slimeHitbox.bounds.Intersects(playerSlamRange.bounds))
            {
                if (!highSlam)
                {
                    //Take one damage on normal slam
                    slime.TakeSlamDamage();
                }
                else
                {
                    //Start hitstop coroutine on high slam
                    StartCoroutine(HitStopTimer());

                    //Freeze time
                    Time.timeScale = 0;

                    //Start post-hitstop coroutine for after hitstop functions
                    StartCoroutine(PostHitStopFunctions(slime));
                }
            }
        }

        if (!highSlam)
        {
            //Generate subtle shake on normal ground slam
            groundImpactShake.GenerateImpulse();
        }
        else
        {
            //Generate strong shake on high grond slam
            highImpactShake.GenerateImpulse();
        }

        //Disable high ground slam variable
        highSlam = false;

        //Play ground impact particles
        playerParticleScript.PlayImpactParticles();
    }

    public void wasHit()
    {
        Debug.Log("Was hit!");

        //Disrupts all special action
        dashCoroutining = false;
        dashJumping = false;
        slideCoroutining = false;
        slideJumping = false;
        slamCoroutining = false;
        highSlam = false;
        
        //Zero player velocity
        velocity = Vector2.zero;

        //Add knockback
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
        velocity.y = knockBackStrength;

        //Add subtle screen shake
        hitShake.GenerateImpulse();

        //Start iframes
        StartCoroutine(IFrames(iFrameDuration));
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
                //Give player higher jump velocity if chained after a slam
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
            }

            if (slideCoroutining)
            {
                //Enable slide jumping if pressed during slide duration
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

            //Generate directional screenshake on dash
            dashShake.GenerateImpulse();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started && isGrounded)
        {
            //Enter sliding state when pressing ctrl on ground
            slideCoroutining = true;

            //Start sliding coroutine
            StartCoroutine(SlideAction());
        }
        else if (context.started && !isGrounded)
        {
            //Enter slamming state when pressing ctrl inair
            slamCoroutining = true;

            //Start slamming coroutine
            StartCoroutine(SlamAction());

            if (transform.position.y > -3)
            {
                //Enable high slam variable if above normal jumping height
                highSlam = true;
            }
        }

        //When releasing ctrl
        if (context.canceled)
        {
            //Disable slidig and slamming state
            slideCoroutining = false;
            slamCoroutining = false;

            //Cancel high slam
            highSlam = false;
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
            //Lock player in facing direction with faster speed when sliding
            velocity.x = facingDirection * slidePower;

            yield return null;
        }
    }

    IEnumerator SlamAction()
    {
        while (slamCoroutining)
        {
            //Lock player horizontally and give constant downwards velocity when slamming
            velocity.y = -slamPower;
            velocity.x = 0;

            yield return null;
        }
    }

    IEnumerator SlamJumpWindow()
    {
        //Timer for slam jump interval
        float slamJumpTimer = slamJumpWindowTime;

        while (slamJumpTimer > 0)
        {
            //Timer countdown during loop
            slamJumpTimer -= Time.deltaTime;

            //Allow slam jumping within time
            canSlamJump = true;

            yield return null;
        }

        //Disable slam jumping window
        canSlamJump = false;
    }

    IEnumerator HitStopTimer()
    {
        //Timer for hitstop interval 
        float hitstopTimer = hitstopDuration;

        //Conditional to diable animation state transitions during hitstop
        exitHitstopState = false;

        while (hitstopTimer > 0)
        {
            //Decrease unaffected timer during time freeze through the loop
            hitstopTimer -= Time.unscaledDeltaTime;

            //Turns on screen flash UI image
            screenFlash.SetActive(true);

            yield return null;
        }

        //Turns off screen flash image after time freeze
        screenFlash.SetActive(false);

        //Set time to normal
        Time.timeScale = 1;
    }

    IEnumerator PostHitStopFunctions(SlimeBehaviour slime)
    {
        while(!(Time.timeScale == 1))
        {
            //Runs only after time has been turned back to normal
            yield return null;
        }

        //Damages slime twice for an instant kill
        slime.TakeSlamDamage();
        slime.TakeSlamDamage();


        //Unblocks animation state transitions
        exitHitstopState = true;
    }

    IEnumerator IFrames(float duration)
    {
        //Timer for iframe duration
        float iFrameTimer = duration;

        //Enable iframes
        iFramesActive = true;

        while (iFrameTimer > 0)
        {
            //Timer countdown
            iFrameTimer -= Time.deltaTime;

            yield return null;
        }

        //Disable iframes
        iFramesActive = false;
    }

    public void DashChargeSystem()
    {
        if (dashCharge < maxDashCharge && !dashCoroutining)
        {
            //Recharges dash when below maximum charges + isn't dashing
            dashCharge +=  Time.deltaTime / dashChargingTime;
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

            if (!isGrounded)
            {
                //Play landing particles when player wasn't grounded upon colliding 
                playerParticleScript.PlayLandParticles();
            }

            //If slamming during ground collision
            if (slamCoroutining)
            {
                //Disable slamming state
                slamCoroutining = false;

                //Start slam jump window
                StartCoroutine(SlamJumpWindow());

                //Deal slam damage
                DoSlamDamage();
            }

            //Enable grounded state
            isGrounded = true;

            //Stop slam + dash/slide jumping velocity
            dashJumping = false;
            slideJumping = false;

            //Lock player to ground height
            Vector2 groundHeight = transform.position;
            groundHeight.y = -5.5f;
            transform.position = groundHeight;
        }
        else
        {
            //Player is not grounded when not colliding with floor
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
