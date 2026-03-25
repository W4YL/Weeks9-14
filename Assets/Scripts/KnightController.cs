using UnityEngine;

public class KnightController : MonoBehaviour
{
    public AudioSource stepSound;
    public ParticleSystem stepParticleSystem;

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
        stepSound.Play();
        stepParticleSystem.Play();
    }
}
