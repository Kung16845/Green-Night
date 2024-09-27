using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
    public List<ComboDeck> comboDecks;
    public List<Lane> lanes;
    private int currentDeckIndex = 0;

    void Start()
    {
        StartCoroutine(ManageDecks());
    }

    IEnumerator ManageDecks()
    {
        while (currentDeckIndex < comboDecks.Count)
        {
            ComboDeck deck = comboDecks[currentDeckIndex];
            StartCoroutine(RunComboDeck(deck));

            yield return new WaitForSeconds(deck.deckDuration);

            currentDeckIndex++;
        }
    }

    IEnumerator RunComboDeck(ComboDeck deck)
    {
        foreach (SpawnInstruction instruction in deck.spawnInstructions)
        {
            StartCoroutine(RunSpawnInstruction(instruction));
        }

        yield return null;
    }

    IEnumerator RunSpawnInstruction(SpawnInstruction instruction)
    {
        int spawned = 0;
        float instructionTimer = instruction.instructionDuration;

        while (spawned < instruction.amountToSpawn && instructionTimer > 0f)
        {
            Lane lane = lanes[instruction.laneIndex];
            GameObject zombieObject = Instantiate(instruction.zombiePrefab, lane.spawnPoint.position, lane.spawnPoint.rotation);

            Zombie zombieScript = zombieObject.GetComponent<Zombie>();
            if (zombieScript != null)
            {
                zombieScript.lane = lane;
                // Set mutation tier or other properties as needed
            }

            spawned++;

            yield return new WaitForSeconds(instruction.timeBetweenSpawns);

            instructionTimer -= instruction.timeBetweenSpawns;
        }
    }
}
