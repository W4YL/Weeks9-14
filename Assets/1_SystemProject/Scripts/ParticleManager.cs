using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem walkParticles;
    public ParticleSystem jumpParticles;
    public ParticleSystem landParticles;
    public ParticleSystem dashParticles;
    public ParticleSystem dashParticles2;
    public ParticleSystem slideParticles;
    public ParticleSystem slamParticles;
    public ParticleSystem slamParticles2;
    public ParticleSystem impactParticles;
    public ParticleSystem impactParticles2;

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

    public void PlayDashParticles()
    {
        dashParticles.Play();
        dashParticles2.Play();
    }

    public void PlaySlideParticles()
    {
        slideParticles.Play();
    }

    public void StopSlideParticles()
    {
        slideParticles.Stop();
    }

    public void PlaySlamParticles()
    {
        slamParticles.Play();
        slamParticles2.Play();
    }

    public void PlayImpactParticles()
    {
        impactParticles.Play();
        impactParticles2.Play();
    }
}
