using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [Header("UI Elements")]
    public TextMeshProUGUI startDelayText;

    [Header("Spawn Timing")]
    public float startDelay = 0f;

    // Variable to determine which decks to use
    public int desiredDeckTier = 1; // Set this in the Inspector or via code

    // **Tracking Variables**
    [Header("Spawn Tracking")]
    public float currentDeckDurationLeft = 0f;     // Time left for the current deck
    public float totalDurationLeft = 0f;           // Total time left for all active decks
    public int totalZombiesLeft = 0;               // Total zombies left to spawn
    public int currentDeckZombiesLeft = 0;         // Zombies left in the current deck

    public List<SpawnDeck> remainingDecks = new List<SpawnDeck>(); // Decks yet to be spawned

    private void Start()
    {
        LaneManager.Instance.RegisterLanes(lanes);
        InitializeSpawnPoints();

        // Initialize saveDataDDA
        saveDataDDA = FindObjectOfType<SaveDataDDA>();

        if (saveDataDDA == null)
        {
            Debug.LogError("SaveDataDDA not found in the scene.");
        }

        // Calculate decks based on DDA setting
        CalculateDecks();

        // Initialize tracking variables
        InitializeTracking();

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
    private IEnumerator CountdownStartDelay()
    {
        float timeLeft = startDelay;
        while (timeLeft > 0)
        {
            if (startDelayText != null)
            {
                startDelayText.text = $"{timeLeft:F1} ";
            }

            yield return new WaitForSeconds(0.1f); // Update every 0.1 seconds for smoother UI
            timeLeft -= 0.1f;
        }
        startDelayText.gameObject.SetActive(false);
        SoundManager.Instance.PlaySound("Siren");
        // SoundManager.Instance.PlaySound("Playmusic");
    }

    private IEnumerator StartSpawningAfterDelay()
    {
        if (startDelay > 0f)
        {
            StartCoroutine(CountdownStartDelay());
            yield return new WaitForSeconds(startDelay);
        }

        StartNextDeck();
    }

    /// <summary>
    /// Calculates and sets the ActiveSpawnDecks based on the DDA setting and ensures total duration <= 360 seconds.
    /// </summary>
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

            // Shuffle the possibleDecks using Fisher-Yates Shuffle
            ShuffleListInPlace(possibleDecks);

            // Add decks until totalDuration reaches 360 seconds
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

    private void ShuffleListInPlace<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            // Swap list[i] with list[j]
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
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

    private float CalculateDDAPoint(float killPerMinute,float accuracy,float barrierDamage,float multikill)
    {
        float DDASkillplayPoint;
        killPerMinute *= 2;
        accuracy *= 1;
        barrierDamage = ((5000 - barrierDamage)/500) * 10;
        multikill *= 5;
        return DDASkillplayPoint = (killPerMinute + accuracy + barrierDamage + multikill);
    }
    private int CalculateDesiredTier(DataDDA avgData)
    {
        float skillpoint = CalculateDDAPoint(avgData.killPerMinute,avgData.accuracy,avgData.barrierDamage,avgData.multiKillCount);
        Debug.Log(skillpoint);
        if (skillpoint >= 320)
        {
            return 4;
        }
        else if (skillpoint >=225)
        {
            return 3;
        }
        else if (skillpoint >= 150)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    /// <summary>
    /// Initializes tracking variables based on ActiveSpawnDecks.
    /// </summary>
    private void InitializeTracking()
    {
        totalDurationLeft = 0f;
        totalZombiesLeft = 0;

        foreach (var deck in ActiveSpawnDecks)
        {
            totalDurationLeft += deck.deckDuration;

            foreach (var wave in deck.spawnWaves)
            {
                foreach (var laneConfig in wave.laneSpawnConfigs)
                {
                    foreach (var zombieQueue in laneConfig.zombieSpawnQueue)
                    {
                        totalZombiesLeft += zombieQueue.quantity;
                    }
                }
            }
        }

        // Initialize remainingDecks
        remainingDecks = new List<SpawnDeck>(ActiveSpawnDecks);

        Debug.Log($"Tracking Initialized: Total Duration Left = {totalDurationLeft}s, Total Zombies Left = {totalZombiesLeft}");
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

            // Update tracking variables
            totalDurationLeft += deckToAdd.deckDuration;

            foreach (var wave in deckToAdd.spawnWaves)
            {
                foreach (var laneConfig in wave.laneSpawnConfigs)
                {
                    foreach (var zombieQueue in laneConfig.zombieSpawnQueue)
                    {
                        totalZombiesLeft += zombieQueue.quantity;
                    }
                }
            }

            remainingDecks.Add(deckToAdd);
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
        // Initialize currentDeckDurationLeft and currentDeckZombiesLeft
        currentDeckDurationLeft = deck.deckDuration;
        currentDeckZombiesLeft = 0;

        Debug.Log($"Processing Deck '{deck.deckName}' with Duration {deck.deckDuration}s.");

        foreach (var wave in deck.spawnWaves)
        {
            foreach (var laneConfig in wave.laneSpawnConfigs)
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

            // Wait for the wave's timeUntilNextWave
            Debug.Log($"Waiting for {wave.timeUntilNextWave}s before next wave.");
            yield return new WaitForSeconds(wave.timeUntilNextWave);
        }

        // Wait for the remaining deck duration
        float elapsedTime = Time.time - (Time.time - deck.deckDuration + currentDeckDurationLeft);
        float remainingDuration = deck.deckDuration - elapsedTime;
        if (remainingDuration > 0f)
        {
            Debug.Log($"Waiting for remaining {remainingDuration}s of deck duration.");
            yield return new WaitForSeconds(remainingDuration);
        }

        Debug.Log($"Deck '{deck.deckName}' completed.");
        currentDeckDurationLeft = 0f;
        currentDeckZombiesLeft = 0;

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

    public void OnZombieSpawned(int zombiesSpawned, float deckDuration)
    {
        totalZombiesLeft -= zombiesSpawned;
        currentDeckZombiesLeft -= zombiesSpawned;

        // Clamp the values to prevent negative numbers
        totalZombiesLeft = Mathf.Max(totalZombiesLeft, 0);
        currentDeckZombiesLeft = Mathf.Max(currentDeckZombiesLeft, 0);

        Debug.Log($"Zombies Spawned: {zombiesSpawned}, Zombies Left: {totalZombiesLeft}");
    }

    public void OnZombieQueueStarted(int zombiesToSpawn, float deckDuration)
    {
        totalZombiesLeft += zombiesToSpawn;
        currentDeckZombiesLeft += zombiesToSpawn;

        Debug.Log($"Zombie Queue Started: {zombiesToSpawn} zombies to spawn.");
    }

    private void Update()
    {
        // Decrement the deck duration
        if (currentDeckDurationLeft > 0f)
        {
            currentDeckDurationLeft -= Time.deltaTime;
            if (currentDeckDurationLeft < 0f)
                currentDeckDurationLeft = 0f;

            // Update the total duration left
            totalDurationLeft = Mathf.Max(totalDurationLeft - Time.deltaTime, 0f);

        }
    }
}
