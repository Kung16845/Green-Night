using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int mutationTier = 1;
    public int amountToSpawn;
    public float timeBetweenSpawns;
    public float spawnDuration;
    public int laneIndex;
    public List<Lane> lanes; // Reference to lanes

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        int spawned = 0;
        float timer = spawnDuration;

        while (spawned < amountToSpawn && timer > 0f)
        {
            // Spawn zombie
            Lane lane = lanes[laneIndex];
            GameObject zombie = Instantiate(zombiePrefab, lane.spawnPoint.position, lane.spawnPoint.rotation);

            // Assign lane and mutation tier to zombie
            Zombie zombieScript = zombie.GetComponent<Zombie>();
            if (zombieScript != null)
            {
                zombieScript.lane = lane;
                zombieScript.mutationTier = mutationTier;
            }

            spawned++;

            // Wait between spawns
            yield return new WaitForSeconds(timeBetweenSpawns);

            timer -= timeBetweenSpawns;
        }
    }
}
