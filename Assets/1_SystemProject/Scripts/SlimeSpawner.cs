using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject spawnedSlime;
    public PlayerController playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
        spawnedSlime.GetComponent<SlimeBehaviour>().InitiateComponents(playerScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
