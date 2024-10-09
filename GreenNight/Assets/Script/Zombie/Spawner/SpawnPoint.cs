using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [HideInInspector]
    public Lane lane;

    [Header("Spawn Configuration")]
    public List<GameObject> zombiePrefabs;
    public float spawnInterval = 5f;
    public int zombieTier = 1;
    public int zombiesToSpawn = 10;

    private int spawnedZombies = 0;
    private Coroutine spawnCoroutine;

    public void Initialize(Lane lane)
    {
        this.lane = lane;
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnedZombies = 0; // Reset count for new spawning sessions
            spawnCoroutine = StartCoroutine(SpawnZombies());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnZombies()
    {
        while (spawnedZombies < zombiesToSpawn)
        {
            SpawnZombie();
            spawnedZombies++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnZombie()
    {
        int index = Random.Range(0, zombiePrefabs.Count);
        GameObject zombiePrefab = zombiePrefabs[index];

        GameObject zombieObject = Instantiate(zombiePrefab, lane.spawnPoint.position, Quaternion.identity);

        Zombie zombie = zombieObject.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.SetLane(lane);
            zombie.SetTier(zombieTier);
        }
    }
}
