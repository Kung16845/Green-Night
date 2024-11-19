using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Lane lane;

    public Coroutine spawnCoroutine;

    public void Initialize(Lane lane)
    {
        this.lane = lane;
    }

    // Starts spawning based on the queue
    public void StartSpawningQueue(List<ZombieSpawnQueue> spawnQueue)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnZombieQueueCoroutine(spawnQueue));
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

    private IEnumerator SpawnZombieQueueCoroutine(List<ZombieSpawnQueue> spawnQueue)
    {
        foreach (ZombieSpawnQueue spawnConfig in spawnQueue)
        {
            int spawnedZombies = 0;
            while (spawnedZombies < spawnConfig.quantity)
            {
                SpawnZombie(spawnConfig);
                spawnedZombies++;
                yield return new WaitForSeconds(spawnConfig.spawnInterval);
            }

            // Optional: Wait before starting next spawn config
            // yield return new WaitForSeconds(optionalDelayBetweenConfigs);
        }

        spawnCoroutine = null; // Reset coroutine so it can be started again if needed
    }

    private void SpawnZombie(ZombieSpawnQueue spawnConfig)
    {
        Debug.Log("Spawned");
        if (spawnConfig.zombiePrefab != null)
        {
            GameObject zombieObject = Instantiate(spawnConfig.zombiePrefab, lane.spawnPoint.position, Quaternion.identity);
            zombieObject.transform.SetParent(lane.spawnPoint.transform);
            Zombie zombie = zombieObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.SetLane(lane);
                zombie.SetTier(spawnConfig.zombieTier);
            }
        }
        else
        {
            Debug.LogWarning("SpawnPoint: zombiePrefab is null in spawnConfig.");
        }
    }
}
