using System.Collections.Generic;
using UnityEngine;

public class DDAdataCollector : MonoBehaviour
{
    public static DDAdataCollector Instance;

    // Public fields to store the data
    public float killPerMinute;
    public float accuracy;
    public int multiKillCount;
    public float barrierDamage;

    // Private fields for calculations
    private int totalKills;
    private int totalBulletsFired;
    private int totalBulletsHit;

    private List<float> killTimestamps;  // For KPM calculation
    private List<float> recentKills;     // For multi-kill tracking

    // Keep track of which bullets have already counted as hits
    private HashSet<int> bulletsThatHit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            killTimestamps = new List<float>();
            recentKills = new List<float>();
            bulletsThatHit = new HashSet<int>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SaveDataDDA saveDataDDA = FindObjectOfType<SaveDataDDA>();
        saveDataDDA.scriptDDAdataCollector = this;

    }
    private void Update()
    {
        UpdateKillPerMinute();
        UpdateAccuracy();
    }

    private void UpdateKillPerMinute()
    {
        float currentTime = Time.time;

        // Remove kills that happened more than 60 seconds ago
        killTimestamps.RemoveAll(t => t < currentTime - 60f);

        // Calculate KPM
        killPerMinute = killTimestamps.Count;
    }

    private void UpdateAccuracy()
    {
        if (totalBulletsFired > 0)
        {
            // Ensure totalBulletsHit does not exceed totalBulletsFired
            int hits = Mathf.Min(totalBulletsHit, totalBulletsFired);
            accuracy = (float)hits / totalBulletsFired * 100f;
        }
        else
        {
            accuracy = 0f;
        }
    }

    // Method to be called when a bullet is fired
    public void OnBulletFired(int bulletID)
    {
        totalBulletsFired++;
        bulletsThatHit.Remove(bulletID); // Ensure it's not already counted
    }

    // Method to be called when a bullet hits a zombie
    public void OnBulletHit(int bulletID)
    {
        // Only increment totalBulletsHit if this bullet hasn't been counted as a hit yet
        if (!bulletsThatHit.Contains(bulletID))
        {
            totalBulletsHit++;
            bulletsThatHit.Add(bulletID);
        }
    }

    // Method to be called when a zombie is killed
    public void OnZombieKilled()
    {
        totalKills++;
        float currentTime = Time.time;

        // Add the kill timestamp for KPM calculation
        killTimestamps.Add(currentTime);

        // Add the kill timestamp for multi-kill tracking
        recentKills.Add(currentTime);

        // Remove kills that happened more than 3 seconds ago for multi-kill
        recentKills.RemoveAll(t => t < currentTime - 3f);

        // Check if we have achieved a multi-kill
        if (recentKills.Count >= 8)
        {
            multiKillCount++;
            recentKills.Clear(); // Reset the recent kills to avoid overlapping counts
        }
    }

    // Method to be called when the barrier takes damage
    public void OnBarrierDamage(float damage)
    {
        barrierDamage += damage;
    }

    // Method to gather all data into a list
    public List<float> GetData()
    {
        List<float> data = new List<float>
        {
            killPerMinute,
            accuracy,
            multiKillCount,
            barrierDamage
        };
        return data;
    }
    
}
