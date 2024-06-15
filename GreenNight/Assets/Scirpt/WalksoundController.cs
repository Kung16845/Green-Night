using System.Collections.Generic;
using UnityEngine;

public class WalksoundController : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;
    private AudioSource audioSource;
    public Animationcontroller animationcontroller;
    public Movement movement;
    // public Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the AudioSource component is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.GetComponent<AudioSource>();
        }
    }

    // Play sound by name
    public void PlaySound(string soundName)
    {
        if (!audioSource.isPlaying)
        {
            Sound sound = sounds.Find(s => s.name == soundName);
            if (sound.clip != null)
            {
                audioSource.clip = sound.clip;
                audioSource.Play();
                // Debug.Log("Playing sound: " + soundName);
            }
            else
            {
                Debug.LogWarning("Sound not found: " + soundName);
            }
        }
        else
        {
            // Debug.Log("AudioSource is already playing a sound.");
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

    // Stop the current sound
    public void StopSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Stopping current sound.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(movement.isWalking)
        {
            PlaySound("Walking");
        }
        if(!movement.isWalking)
        {
            StopSound();
        }
    }
}
