using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnWave
{
    public int laneID;
    public List<GameObject> zombiePrefabs;
    public int zombiesToSpawn;
    public float spawnInterval;
    public int zombieTier;
    public float timeUntilNextWave;
}
