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
        SlimeBehaviour slime = slimePrefab.GetComponent<SlimeBehaviour>();

        slime.InitiateComponents(playerScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
