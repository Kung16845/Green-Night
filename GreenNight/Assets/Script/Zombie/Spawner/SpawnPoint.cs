using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Lane lane;

    public Coroutine spawnCoroutine;
    private MainSpawner mainSpawner;

    public void Initialize(Lane lane)
    {
        this.lane = lane;
        mainSpawner = FindObjectOfType<MainSpawner>();
        if (mainSpawner == null)
        {
            Debug.LogError("MainSpawner not found in the scene!");
        }
    }

    // Starts spawning based on the queue
     public void StartSpawningQueue(List<ZombieSpawnQueue> spawnQueue, MutationType mutationType, float mutationApplyRate)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnZombieQueueCoroutine(spawnQueue, mutationType, mutationApplyRate));
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

    private IEnumerator SpawnZombieQueueCoroutine(List<ZombieSpawnQueue> spawnQueue, MutationType mutationType, float mutationApplyRate)
    {
        foreach (ZombieSpawnQueue spawnConfig in spawnQueue)
        {
            // Notify MainSpawner about zombies to spawn
            mainSpawner?.OnZombieQueueStarted(spawnConfig.quantity, spawnConfig.spawnInterval);

            int spawnedZombies = 0;
            while (spawnedZombies < spawnConfig.quantity)
            {
                SpawnZombie(spawnConfig, mutationType, mutationApplyRate);
                spawnedZombies++;
                yield return new WaitForSeconds(spawnConfig.spawnInterval);
            }

            // Notify MainSpawner about zombies spawned
            mainSpawner?.OnZombieSpawned(spawnedZombies, spawnConfig.spawnInterval);
        }

        spawnCoroutine = null; // Reset coroutine so it can be started again if needed
    }
     private void SpawnZombie(ZombieSpawnQueue spawnConfig, MutationType mutationType, float mutationApplyRate)
    {
        if (spawnConfig.zombiePrefab != null)
        {
            GameObject zombieObject = Instantiate(spawnConfig.zombiePrefab, lane.spawnPoint.position, Quaternion.identity);
            zombieObject.transform.SetParent(lane.spawnPoint.transform);

            Zombie zombie = zombieObject.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.SetLane(lane);
                zombie.SetTier(spawnConfig.zombieTier);

                // Apply mutation based on the mutationApplyRate
                if (Random.value <= mutationApplyRate)
                {
                    zombie.SetMutationType(mutationType);
                }
                else
                {
                    zombie.SetMutationType(MutationType.None);
                }

            }
        }
        else
        {
            Debug.LogWarning("SpawnPoint: zombiePrefab is null in spawnConfig.");
        }
    }
}
