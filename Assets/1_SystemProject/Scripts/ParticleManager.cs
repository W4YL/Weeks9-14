using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem walkParticles;
    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayWalkParticles()
    {
        walkParticles.Play();
    }

    public void PlayJumpParticles()
    {
        jumpParticles.Play();
    }

    public void PlayLandParticles()
    {
        landParticles.Play();
    }
}
