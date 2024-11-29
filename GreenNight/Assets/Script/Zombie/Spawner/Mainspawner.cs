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

    [Header("Global Mutation Settings")]
    public MutationType globalMutationType = MutationType.None;
    [Range(0f, 1f)]
    public float mutationApplyRate = 0f;

    [Header("DDA Settings")]
    public bool useDDA = false;  // Toggle to enable/disable DDA system

    private SaveDataDDA saveDataDDA;  // Reference to SaveDataDDA script
    private int currentDeckIndex;
    private Coroutine deckCoroutine;

    [Header("Spawn Timing")]
    public float startDelay = 0f;

    // Variable to determine which decks to use
    public int desiredDeckTier = 1; // Set this in the Inspector or via code

    private void Start()
    {
        LaneManager.Instance.RegisterLanes(lanes);
        InitializeSpawnPoints();

        // Initialize saveDataDDA
        saveDataDDA = FindObjectOfType<SaveDataDDA>();

        // Calculate decks based on DDA setting
        CalculateDecks();

        currentDeckIndex = 0;
        StartCoroutine(StartSpawningAfterDelay());
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

    private IEnumerator StartSpawningAfterDelay()
    {
        Debug.Log("StartCount.");
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }

        StartNextDeck();
    }

    public void CalculateDecks()
    {
        ActiveSpawnDecks.Clear();
        float totalDuration = 0f;

        if (useDDA && saveDataDDA != null && saveDataDDA.dataCollection != null)
        {
            DataDDA avgData = saveDataDDA.dataCollection.averageData;
            desiredDeckTier = CalculateDesiredTier(avgData);
            Debug.Log($"DDA enabled. Desired deck tier: {desiredDeckTier}");

            // Find decks matching the desiredTier
            List<SpawnDeck> possibleDecks = StorageDecks.FindAll(deck => deck.deckTier == desiredDeckTier);

            if (possibleDecks.Count == 0)
            {
                Debug.LogWarning($"No decks found with tier {desiredDeckTier}. Falling back to random selection.");
                SelectRandomDecks(ref ActiveSpawnDecks, 360f, StorageDecks);
                return;
            }

            // Sort decks by deckDuration descending to maximize duration utilization
            possibleDecks.Sort((a, b) => b.deckDuration.CompareTo(a.deckDuration));

            foreach (var deck in possibleDecks)
            {
                if (totalDuration + deck.deckDuration <= 360f)
                {
                    ActiveSpawnDecks.Add(deck);
                    totalDuration += deck.deckDuration;
                }

                if (totalDuration >= 360f)
                    break;
            }

            Debug.Log($"Selected {ActiveSpawnDecks.Count} decks with total duration {totalDuration} seconds.");
        }
        else
        {
            Debug.Log("DDA disabled. Selecting decks randomly.");
            SelectRandomDecks(ref ActiveSpawnDecks, 360f, StorageDecks);
        }

        // Optional: If ActiveSpawnDecks is empty after selection, handle accordingly
        if (ActiveSpawnDecks.Count == 0)
        {
            Debug.LogWarning("No decks selected. Please check StorageDecks and selection criteria.");
        }
    }

    private void SelectRandomDecks(ref List<SpawnDeck> selectedDecks, float maxDuration, List<SpawnDeck> deckPool)
    {
        selectedDecks.Clear();
        float totalDuration = 0f;

        if (deckPool == null || deckPool.Count == 0)
        {
            Debug.LogError("Deck pool is empty or null. Cannot select decks.");
            return;
        }

        List<SpawnDeck> availableDecks = new List<SpawnDeck>(deckPool);
        System.Random rand = new System.Random();

        while (availableDecks.Count > 0 && totalDuration < maxDuration)
        {
            int index = rand.Next(availableDecks.Count);
            SpawnDeck selectedDeck = availableDecks[index];

            if (totalDuration + selectedDeck.deckDuration <= maxDuration)
            {
                selectedDecks.Add(selectedDeck);
                totalDuration += selectedDeck.deckDuration;
            }

            // Remove the selected deck to avoid duplication
            availableDecks.RemoveAt(index);
        }

        Debug.Log($"Randomly selected {selectedDecks.Count} decks with total duration {totalDuration} seconds.");
    }


    private int CalculateDesiredTier(DataDDA avgData)
    {
        if (avgData.killPerMinute >= 188 && avgData.accuracy >= 80 && avgData.barrierDamage <= 1000 && avgData.multiKillCount >= 18)
        {
            return 4;
        }
        else if (avgData.killPerMinute >= 150 && avgData.accuracy >= 70 && avgData.barrierDamage <= 1500 && avgData.multiKillCount >= 15)
        {
            return 3;
        }
        else if (avgData.killPerMinute >= 100 && avgData.accuracy >= 65 && avgData.barrierDamage <= 2500 && avgData.multiKillCount >= 10)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }


    // Existing AddActiveDeck method remains unchanged
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
        Debug.Log("StartNextDeck.");
        if (currentDeckIndex < ActiveSpawnDecks.Count)
        {
            SpawnDeck currentDeck = ActiveSpawnDecks[currentDeckIndex];
            StartCoroutine(ProcessDeck(currentDeck));
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
            foreach (LaneSpawnConfig laneConfig in wave.laneSpawnConfigs)
            {
                SpawnPoint spawnPoint = GetSpawnPointByLaneID(laneConfig.laneID);

                if (spawnPoint != null)
                {
                    spawnPoint.StartSpawningQueue(laneConfig.zombieSpawnQueue, globalMutationType, mutationApplyRate);
                }
                else
                {
                    Debug.LogWarning($"No spawn point found for lane ID {laneConfig.laneID}");
                }
            }

            yield return new WaitForSeconds(wave.timeUntilNextWave);
        }

        if (deck.deckDuration > 0f)
        {
            yield return new WaitForSeconds(deck.deckDuration);
        }

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
