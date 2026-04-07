using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slimePrefab;
    public List<GameObject> spawnedSlimes = new List<GameObject>();
    public PlayerController player;
    public SlimeAnimation slimeAnimation;

    public SpriteRenderer spawnCheckLeft;
    public SpriteRenderer spawnCheckRight;

    Vector2 spawnerPosition;
    public float spawnTimer = 0;

    public float spawnMaxInterval = 2;
    public float spawnZoneWidth = 17.5f;
    public int spawnZone = 2;
    public int maxSlimeCount = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        SpawnerPositionLogic();
        SpawnZoneCheck();

        if (spawnTimer <= 0)
        {
            SpawningLogic();
            spawnTimer = Random.Range(1, spawnMaxInterval);
        }
    }

    public void SpawnerPositionLogic()
    {
        if (spawnZone == 1)
        {
            spawnerPosition.x = -30;
        }
        else if (spawnZone == 2)
        {
            spawnerPosition.x = 0;
        }
        else if (spawnZone == 3)
        {
            spawnerPosition.x = 30;
        }
    }

    public void SpawningLogic()
    {
        if (spawnedSlimes.Count < maxSlimeCount)
        {
            if (spawnZone == 1 || spawnZone == 3)
            {
                SpawnSlime(Random.Range(transform.position.x - spawnZoneWidth / 2, transform.position.x + spawnZoneWidth / 2));
            }
            if (spawnZone == 2)
            {
                //Random value between two numbers: https://discussions.unity.com/t/randomly-pick-between-two-numbers/62601/3
                float spawnPosition;


                if (Random.value < 0.5)
                {
                    spawnPosition = -38;
                }
                else
                {
                    spawnPosition = 38;
                }

                SpawnSlime(spawnPosition);
            }
        }
    }

    public void SpawnZoneCheck()
    {
        if (!player.playerHitbox.bounds.Intersects(spawnCheckRight.bounds))
        {
            spawnZone = 1;
        }
        else if (!player.playerHitbox.bounds.Intersects(spawnCheckLeft.bounds))
        {
            spawnZone = 3;
        }
        else
        {
            spawnZone = 2;
        }
    }

    public void SpawnSlime(float spawnPosition)
    {
        GameObject spawnedSlime = Instantiate(slimePrefab, new Vector2(spawnPosition, transform.position.y), Quaternion.identity);
        SlimeBehaviour slimeScript = spawnedSlime.GetComponent<SlimeBehaviour>();
        slimeScript.spawner = GetComponent<SlimeSpawner>();

        slimeScript.InitiateComponents(player, slimeAnimation);
        spawnedSlimes.Add(spawnedSlime);
    }
}
