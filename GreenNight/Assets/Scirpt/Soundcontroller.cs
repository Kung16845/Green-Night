using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundController : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;
    public AudioSource audioSource;
    public Animationcontroller animationcontroller;
    public Movement movement;
    // public Gun gun;
    public bool soundisplayed;

    private Dictionary<string, float> lastPlayTime;
    public float assaultRifleCooldown; // Cooldown duration in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the AudioSource component is attached
        assaultRifleCooldown = 0.15f;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.GetComponent<AudioSource>();
        }
        soundisplayed = false;
        
        // Initialize the last play time dictionary
        lastPlayTime = new Dictionary<string, float>();
    }

    // Play sound by name with optional cooldown
    public void PlaySound(string soundName, float cooldown = 0f)
    {
        Sound sound = sounds.Find(s => s.name == soundName);
        if (sound.clip != null)
        {
            if (!lastPlayTime.ContainsKey(soundName) || Time.time - lastPlayTime[soundName] >= cooldown)
            {
                audioSource.clip = sound.clip;
                audioSource.Play();
                lastPlayTime[soundName] = Time.time;
                Debug.Log("Playing sound: " + soundName);
            }
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

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(gun.currentAmmo);
        if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha1))
        {
            PlaySound("Changegun");
        }
        switch (animationcontroller.Whichgun)
        {
            case 1: 
                HandleGunSound("M4gunshot", "ReloadAR", "CocksoundAR", assaultRifleCooldown);
                break;
            case 2: 
                HandleGunSound("ShootingShotgun", "ReloadShotgun", "CocksoundShotgun");
                break;
            case 3: 
                HandleGunSound("Shootingpistol", "ReloadPistol", "Cocksoundpistol");
                break;
        }
    }

    // void HandleGunSound(string shootSound, string reloadSound, string cockSound, float shootCooldown = 0f)
    // {
    //     if (animationcontroller.Isshoot)
    //     {
    //         if (gun.currentAmmo > 0)
    //         {
    //             PlaySound(shootSound, shootCooldown);
    //         }
    //         else if (gun.currentAmmo == 0)
    //         {
    //             if (Input.GetKeyDown(KeyCode.Mouse0))
    //             {
    //                 Debug.Log("Cocksoundpistol");
    //                 PlaySound(cockSound);
    //             }
    //         }
    //     }
    //     else if (Input.GetKey(KeyCode.R) && gun.isReload)
    //     {
    //         PlaySound(reloadSound);
    //     }
    // }
}
