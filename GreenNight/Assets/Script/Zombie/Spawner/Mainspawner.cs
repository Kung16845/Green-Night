using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
    [Header("Lanes Configuration")]
    public List<Lane> lanes = new List<Lane>();

    [Header("Spawn Decks Configuration")]
    public List<SpawnDeck> spawnDecks = new List<SpawnDeck>();

    private int currentDeckIndex = 0;
    private Coroutine deckCoroutine;

    private void Start()
    {
        InitializeSpawnPoints();
        StartNextDeck();
    }

    private void InitializeSpawnPoints()
    {
        foreach (Lane lane in lanes)
        {
            // Ensure the spawn point has a SpawnPoint component
            SpawnPoint spawnPoint = lane.spawnPoint.GetComponent<SpawnPoint>();
            if (spawnPoint == null)
            {
                spawnPoint = lane.spawnPoint.gameObject.AddComponent<SpawnPoint>();
            }

            spawnPoint.Initialize(lane);
        }
    }

    private void StartNextDeck()
    {
        if (currentDeckIndex < spawnDecks.Count)
        {
            SpawnDeck currentDeck = spawnDecks[currentDeckIndex];
            deckCoroutine = StartCoroutine(ProcessDeck(currentDeck));
        }
        else
        {
            Debug.Log("All spawn decks have been completed.");
        }
    }

    private IEnumerator ProcessDeck(SpawnDeck deck)
    {
        foreach (SpawnWave wave in deck.spawnWaves)
        {
            SpawnPoint spawnPoint = GetSpawnPointByLaneID(wave.laneID);

            if (spawnPoint != null)
            {
                spawnPoint.zombiePrefabs = wave.zombiePrefabs;
                spawnPoint.spawnInterval = wave.spawnInterval;
                spawnPoint.zombiesToSpawn = wave.zombiesToSpawn;
                spawnPoint.zombieTier = wave.zombieTier;

                spawnPoint.StartSpawning();
            }
            else
            {
                Debug.LogWarning($"No spawn point found for lane ID {wave.laneID}");
            }

            yield return new WaitForSeconds(wave.timeUntilNextWave);
        }

        // Wait for the deck's total duration
        yield return new WaitForSeconds(deck.deckDuration);

        // Move to the next deck
        currentDeckIndex++;
        StartNextDeck();
    }

    private SpawnPoint GetSpawnPointByLaneID(int laneID)
    {
        Lane lane = lanes.Find(l => l.laneID == laneID);
        if (lane != null)
        {
            return lane.spawnPoint.GetComponent<SpawnPoint>();
        }
        return null;
    }
}
