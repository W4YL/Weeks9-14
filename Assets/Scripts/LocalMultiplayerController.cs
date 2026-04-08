using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerController : MonoBehaviour
{
    public LocalMultiplayerManager manager;
    public PlayerInput playerInput;
    public Animator animator;
    public Vector2 movementInput;
    public float speed = 5f;
    public ParticleSystem hitParticles;
    public bool isDashing;
    public float dashDuration = 1;
    public float dashSpeedMultiplier = 7f;

    public TrailRenderer dashTrail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            speed = dashSpeedMultiplier;
        }
        else
        {
            speed = 5;
        }

        transform.position += (Vector3)movementInput * speed * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Player" + playerInput.playerIndex + ": Attacking");

            manager.PlayerAttacking(playerInput);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("dashing");
            StartCoroutine(DashAction());

        }
    }

    public void GotHit()
    {
        animator.SetTrigger("Squish");
        hitParticles.Play();
    }

    IEnumerator DashAction()
    {
        float timer = dashDuration;
        dashTrail.emitting = true;

        isDashing = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isDashing = false;

        dashTrail.emitting = false;
    }
}
