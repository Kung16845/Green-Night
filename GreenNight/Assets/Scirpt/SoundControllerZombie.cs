using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControllerZombie : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;
    public AudioSource audioSource;
    private bool soundIsPlayed;
    public Enemy enemy; // Reference to the base Enemy class
    private float lastHp;

    void Start()
    {
        // Ensure the AudioSource component is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        soundIsPlayed = false;
        lastHp = enemy.currentHp; // Initialize lastHp with the current health of the enemy
    }

    // Play sound by name
    public void PlaySound(string soundName)
    {
        // if (audioSource.isPlaying) return; // Prevent playing sound if another is playing

        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound.clip != null)
        {
            audioSource.clip = sound.clip;
            audioSource.Play();
            Debug.Log("Playing sound: " + soundName);
        }
        else
        {
            Debug.LogWarning("Sound not found: " + soundName);
        }
    }

    // Add a new sound to the list
    public void AddSound(string soundName, AudioClip clip)
    {
        if (!sounds.Exists(s => s.name == soundName))
        {
            Sound newSound = new Sound
            {
                name = soundName,
                clip = clip
            };
            sounds.Add(newSound);
        }
        else
        {
            Debug.LogWarning("Sound with the same name already exists: " + soundName);
        }
    }

    // Stop currently playing sound
    public void StopSound()
    {
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null)
        {
            HandleZombieSounds();
        }
    }

    // Handle sounds based on the type of zombie and its state
    void HandleZombieSounds()
    {
        if (enemy.currentHp < lastHp && !soundIsPlayed)
        {
            if (enemy is EnemySpeedter)
            {
                PlaySound("Takendamage");
            }
            else if (enemy is EnemyGrunt)
            {
                PlaySound("Takendamage");
            }
            // Add other zombie types here as needed
            // else if (enemy is AnotherZombieType)
            // {
            //     PlaySound("AnotherZombieTypeDamageSound");
            // }
            soundIsPlayed = true;
        }

        if (enemy.currentHp == lastHp)
        {
            soundIsPlayed = false; // Reset soundIsPlayed when health is unchanged
        }

        lastHp = enemy.currentHp; // Update lastHp to the current health after processing
    }
}
