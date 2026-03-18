using UnityEngine;
using UnityEngine.InputSystem;

public class FlowerSpawner : MonoBehaviour
{
    public GameObject flowerFarm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Instantiate(flowerFarm, mousePos, Quaternion.identity);
        }
    }
}
