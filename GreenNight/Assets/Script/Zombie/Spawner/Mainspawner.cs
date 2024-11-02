using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
    [Header("Lanes Configuration")]
    public List<Lane> lanes = new List<Lane>();

    [Header("Spawn Decks Configuration")]
    public List<SpawnDeck> ActiveSpawnDecks = new List<SpawnDeck>();  // Active decks being used
    public List<SpawnDeck> StorageDecks = new List<SpawnDeck>();      // Saved decks waiting to be used

    private int currentDeckIndex = 0;
    private Coroutine deckCoroutine;

    // Variable to determine which decks to use
    public int desiredDeckTier = 1; // Set this in the Inspector or via code

    private void Start()
    {
        LaneManager.Instance.RegisterLanes(lanes);
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

    // Function to calculate which decks to use based on the desired tier
    public void CalculateDecksToUse(int desiredTier)
    {
        // Clear the current ActiveSpawnDecks
        ActiveSpawnDecks.Clear();

        // Find decks in StorageDecks that match the desiredTier
        List<SpawnDeck> decksToAdd = StorageDecks.FindAll(deck => deck.deckTier == desiredTier);

        if (decksToAdd.Count > 0)
        {
            ActiveSpawnDecks.AddRange(decksToAdd);
            Debug.Log($"Added {decksToAdd.Count} decks of tier {desiredTier} to ActiveSpawnDecks.");
        }
        else
        {
            Debug.LogWarning($"No decks found with tier {desiredTier} in StorageDecks.");
        }
    }

    // Function to add a specific deck to ActiveSpawnDecks by deckID
    public void AddActiveDeck(int deckID)
    {
        // Find the deck with the given deckID in StorageDecks
        SpawnDeck deckToAdd = StorageDecks.Find(deck => deck.deckID == deckID);
        if (deckToAdd != null)
        {
            ActiveSpawnDecks.Add(deckToAdd);
            Debug.Log($"Added deck '{deckToAdd.deckName}' (ID: {deckID}) to ActiveSpawnDecks.");
        }
        else
        {
            Debug.LogWarning($"Deck with ID {deckID} not found in StorageDecks.");
        }
    }

    private void StartNextDeck()
    {
        if (currentDeckIndex < ActiveSpawnDecks.Count)
        {
            SpawnDeck currentDeck = ActiveSpawnDecks[currentDeckIndex];
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
                // Set spawn parameters
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
