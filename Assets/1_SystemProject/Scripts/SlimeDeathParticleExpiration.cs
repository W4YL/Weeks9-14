using UnityEngine;

public class SlimeDeathParticleExpiration : MonoBehaviour
{
    //Expiration timer value
    public float timer = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Timer countdown
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            //Destroy prefab when timer hits 0
            Destroy(gameObject);
        }
    }
}
