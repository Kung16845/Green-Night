using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawnInstruction
{
    public GameObject zombiePrefab;          // Prefab of the zombie to spawn
    public int amountToSpawn;                // Number of zombies to spawn
    public int laneIndex;                    // Lane index to spawn zombies in
    public float timeBetweenSpawns;          // Time between spawns
    public float instructionDuration;        // Duration of this spawn instruction
}

[System.Serializable]
public class ComboDeck
{
    public List<SpawnInstruction> spawnInstructions;
    public float deckDuration;               // Duration before moving to the next deck
}
