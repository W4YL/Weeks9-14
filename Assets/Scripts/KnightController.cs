using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnightController : MonoBehaviour
{
    public AudioSource stepSound;
    public ParticleSystem stepParticleSystem;
    public List<AudioClip> stepSoundClips;
    public Animator knightAnimator;

    Vector2 inputVector;
    public Vector2 movement;
    public Vector2 mousePos;
    public Vector2 savedPos;
    public float speed = 4;
    Coroutine movePlayerCoroutine;
    bool coroutining = false;

    int facingDirection = 1;

    public AnimationCurve moveCurve;

    public Vector2 flipScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flipScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += (Vector3)movement * speed * Time.deltaTime;
        //transform.position = movement;

        flipScale.x = facingDirection;
        transform.localScale = flipScale;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        mousePos = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartPlayerMovement();
        }
    }

    public void StartPlayerMovement()
    {
        if (!coroutining)
        {
            movePlayerCoroutine = StartCoroutine(MoveToPosition());
        }
    }

    IEnumerator MoveToPosition()
    {
        coroutining = true;
        savedPos = mousePos;

        Debug.Log("Coroutine called");
        float t = 0;

        Vector2 startPos = movement;
        Vector2 endPos = savedPos;

        while (movement != savedPos && t < 1)
        {
            t += Time.deltaTime / Vector2.Distance(startPos, endPos) * speed;
            transform.position = Vector2.Lerp(startPos, endPos, t);

            knightAnimator.SetBool("isMoving", true);

            if (startPos.x > endPos.x)
            {
                facingDirection = -1;
            }
            else if (startPos.x < endPos.x)
            {
                facingDirection = 1;
            }

                yield return null;
        }

        knightAnimator.SetBool("isMoving", false);
        movement = savedPos;

        coroutining = false;
    }

    public void FootStep()
    {
        Debug.Log("Step!");
        stepParticleSystem.Emit(10);

        stepSound = transform.GetComponent<AudioSource>();
        int randomNum;
        randomNum = Random.Range(0, 4);
        stepSound.clip = stepSoundClips[randomNum];
        stepSound.pitch = Random.Range(0.5f, 4);
        stepSound.PlayOneShot(stepSound.clip);
    }
}
