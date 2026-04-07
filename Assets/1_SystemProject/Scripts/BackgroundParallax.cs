using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    //Get reference from camera position
    public Camera cameraObject;

    //Parallax multiplier 
    public float parallaxValue;

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
        //Position follows camera with the public multiplier 
        newPosition.x = cameraObject.transform.position.x * parallaxValue;
        transform.position = newPosition;
    }
}
