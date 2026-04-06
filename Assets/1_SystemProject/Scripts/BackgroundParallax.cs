using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Camera cameraObject;
    public float parallaxValue;
    Vector2 newPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPosition.x = cameraObject.transform.position.x * parallaxValue;
        transform.position = newPosition;
    }
}
