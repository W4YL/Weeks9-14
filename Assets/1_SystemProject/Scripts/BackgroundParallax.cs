using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    //Get reference from camera position
    public Camera cameraObject;

    //Parallax multiplier 
    public float parallaxValue;
    public float cloudSpeed = 0;

    float cloudMovement = 0;

    //New camera-following position
    Vector2 newPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize new position
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cloudMovement -= Time.deltaTime * cloudSpeed;

        if (cloudMovement <= -77.5f)
        {
            cloudMovement = 0;
        }

        //Position follows camera with the public multiplier 
        newPosition.x = cameraObject.transform.position.x * parallaxValue + cloudMovement;
        transform.position = newPosition;
    }
}
