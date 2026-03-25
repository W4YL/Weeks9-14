using System.Collections.Generic;
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
    public float speed = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        //Get input as a Vector2 where 0 is no input, 1 is input
        inputVector = Vector2.zero;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            //move left
            inputVector.x -= 1;
        }
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            //move right
            inputVector.x += 1;
        }

        //multiply input by speed and Time
        movement = inputVector * speed * Time.deltaTime;
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
