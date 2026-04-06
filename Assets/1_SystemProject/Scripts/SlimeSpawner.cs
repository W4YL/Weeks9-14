using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject spawnedSlime;
    public PlayerController playerScript;

    public SpriteRenderer spawnZoneLeft;
    public SpriteRenderer spawnZoneRight;

    public int spawnZone = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedSlime = Instantiate(slimePrefab, transform.position, Quaternion.identity);
        SlimeBehaviour slime = spawnedSlime.GetComponent<SlimeBehaviour>();

        slime.InitiateComponents(playerScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
