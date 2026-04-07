using UnityEngine;

public class SlimeParticles : MonoBehaviour
{
    //Reference to particle object
    public ParticleSystem hitParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayHitParticles()
    {
        //Function to play particles when called by the slime's take damage event
        hitParticles.Play();
    }
}
