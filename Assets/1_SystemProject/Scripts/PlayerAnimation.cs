using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController player;
    public Animator playerAnimator;
    public SpriteRenderer playerSr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Flip player according to turn direction
        if (player.facingDirection < 0)
            playerSr.flipX = true;
        else
            playerSr.flipX = false;

        //Check if player is walking
        if (Mathf.Abs(player.velocity.x) == player.speed)
            playerAnimator.SetBool("isWalking", true);
        else
            playerAnimator.SetBool("isWalking", false);

        //Check if player is floating
        if (player.velocity.y != 0)
            playerAnimator.SetBool("isFloating", true);
        else
            playerAnimator.SetBool("isFloating", false);

        //Check if player if falling
        if (player.velocity.y < 0)
            playerAnimator.SetBool("isFalling", true);
        else
            //If not, play jump animation
            playerAnimator.SetBool("isFalling", false);

        //Check dashing state
        if (player.dashCoroutining && !player.dashJumping)
            playerAnimator.SetBool("isDashing", true);
        else
            playerAnimator.SetBool("isDashing", false);

        //Check sliding state
        if (player.slideCoroutining && !player.slideJumping)
            playerAnimator.SetBool("isSliding", true);
        else
            playerAnimator.SetBool("isSliding", false);

        //Check slamming state
        if (player.slamCoroutining)
            playerAnimator.SetBool("isSlamming", true);
        else
            playerAnimator.SetBool("isSlamming", false);
    }
}
