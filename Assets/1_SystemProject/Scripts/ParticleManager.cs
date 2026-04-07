using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //Every particle reference
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
        //Function to play walk particles on animation event
        walkParticles.Play();
    }

    public void PlayJumpParticles()
    {
        //Function to play jump particles on animation event
        jumpParticles.Play();
    }

    public void PlayLandParticles()
    {
        //Function to player landing particles when player is grounded initially
        landParticles.Play();
    }

    public void PlayDashParticles()
    {
        //Play two different dash particles on animation event
        dashParticles.Play();
        dashParticles2.Play();
    }

    public void PlaySlideParticles()
    {
        //Start slide particle loop when player is sliding on the ground
        slideParticles.Play();
    }

    public void StopSlideParticles()
    {
        //Stop slide particle loop when player leaves ground slide state
        slideParticles.Stop();
    }

    public void PlaySlamParticles()
    {
        //Play two different start slam particles on animation event
        slamParticles.Play();
        slamParticles2.Play();
    }

    public void PlayImpactParticles()
    {
        //Play two different slam impact particles when becoming grounded during slamming state
        impactParticles.Play();
        impactParticles2.Play();
    }
}
