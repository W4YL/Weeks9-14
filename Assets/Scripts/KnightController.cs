using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public AudioSource stepSound;
    public ParticleSystem stepParticleSystem;
    public List<AudioClip> stepSoundClips;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
