using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    //Slime prefab references
    public GameObject slimePrefab;
    public List<GameObject> spawnedSlimes = new List<GameObject>();

    //Player script reference
    public PlayerController player;

    //Spawning location hitbox checks
    public SpriteRenderer spawnCheckLeft;
    public SpriteRenderer spawnCheckRight;

    //Spawner stats
    public Vector2 spawnerPosition;
    public float spawnTimer = 0;

    //Balancing stats
    public float spawnMaxInterval = 2;
    public float spawnZoneWidth = 17.5f;
    public int spawnZone = 2;
    public int maxSlimeCount = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize new spawner position to the spawner's location
        spawnerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Spawn timer countdown
        spawnTimer -= Time.deltaTime;

        //Check for player location
        SpawnZoneCheck();

        //Determines exact spawning range
        SpawnerPositionLogic();

        //When timer hits 0
        if (spawnTimer <= 0)
        {
            //Start spawning logic
            SpawningLogic();

            //Set random respawn time
            spawnTimer = Random.Range(1, spawnMaxInterval);
        }
    }

    public void SpawnZoneCheck()
    {
        if (!player.playerHitbox.bounds.Intersects(spawnCheckRight.bounds))
        {
            //Set leftmost spawning zone active when player is not intersecting with the right collision zone
            spawnZone = 1;
        }
        else if (!player.playerHitbox.bounds.Intersects(spawnCheckLeft.bounds))
        {
            //Set rightmost spawning zone active when player is not intersecting with the left collision zone
            spawnZone = 3;
        }
        else
        {
            //Set center spawning zone active when player intersects with both collision zones
            spawnZone = 2;
        }
    }

    public void SpawnerPositionLogic()
    {
        if (spawnZone == 1)
        {
            //Spawner placed on left when left spawning zone is active
            spawnerPosition.x = -30;
        }
        else if (spawnZone == 2)
        {
            //Spawner placed in the middle when center zone is active
            spawnerPosition.x = 0;
        }
        else if (spawnZone == 3)
        {
            //Spawner placed on right when left spawning zone is active
            spawnerPosition.x = 30;
        }
    }

    public void SpawningLogic()
    {
        //If the number of slimes spawned hasn't hit the max cap
        if (spawnedSlimes.Count < maxSlimeCount)
        {
            if (spawnZone == 1 || spawnZone == 3)
            {
                //Spawn slimes in a range of the spawner location if spawner is on the left or right
                SpawnSlime(Random.Range(transform.position.x - spawnZoneWidth / 2, transform.position.x + spawnZoneWidth / 2));
            }
            if (spawnZone == 2)
            {
                //Random value between two numbers: https://discussions.unity.com/t/randomly-pick-between-two-numbers/62601/3
                float spawnPosition;

                //Check for two different outcomes from a randomized range between 0-1
                if (Random.value < 0.5)
                {
                    //Position on left boundary
                    spawnPosition = -38;
                }
                else
                {
                    //Position on right boundary
                    spawnPosition = 38;
                }

                //Spawn slimes in one of the two positions
                SpawnSlime(spawnPosition);
            }
        }
    }

    public void SpawnSlime(float spawnPosition)
    {
        //Instantiate a slime prefab on the pre-determined spawn positions
        GameObject spawnedSlime = Instantiate(slimePrefab, new Vector2(spawnPosition, transform.position.y), Quaternion.identity);

        //Give each slime a behavioural script
        SlimeBehaviour slimeScript = spawnedSlime.GetComponent<SlimeBehaviour>();

        //Give the slime script a reference to the spawner (for removing list items)
        slimeScript.spawner = GetComponent<SlimeSpawner>();

        //Give the slime script a referemce to the player script to pull extra references out of
        slimeScript.InitiateComponents(player);

        //Add a slime to the spawned list
        spawnedSlimes.Add(spawnedSlime);
    }
}
