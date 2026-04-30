using UnityEngine;

public class CloudMotion : MonoBehaviour
{
    public float speed = 1;
    Vector2 newPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPos.x -= Time.deltaTime * speed;

        if (transform.position.x <= -77.5f)
        {
            newPos.x = 0;
        }

        transform.position = newPos;
    }
}
