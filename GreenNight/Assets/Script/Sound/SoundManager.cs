using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGSound,
    MusicSound,
    VFXSound,
    Gunshot
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public SoundType soundType;
    public float cooldown; // Minimum time between plays
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound List")]
    public List<Sound> sounds = new List<Sound>();
    private Dictionary<string, float> soundCooldowns = new Dictionary<string, float>();

    [Header("Audio Source Pool")]
    public int poolSize = 30;
    private List<AudioSource> audioSourcePool;
    private List<AudioSource> VFXaudioSourcePool;
    private int currentSourceIndex = 0;
    private int VFXcurrentSourceIndex = 0;
    private AudioSource bgAudioSource;
    private AudioSource musicAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Dedicated sources for BG and Music
            bgAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource = gameObject.AddComponent<AudioSource>();

            // Initialize audio source pool
            audioSourcePool = new List<AudioSource>();
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                audioSourcePool.Add(source);
            }
            VFXaudioSourcePool = new List<AudioSource>();
            for (int i = 0; i < poolSize; i++)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                VFXaudioSourcePool.Add(source);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            // Check cooldown
            if (soundCooldowns.TryGetValue(name, out float lastPlayedTime))
            {
                if (Time.time - lastPlayedTime < sound.cooldown)
                {
                    return; // Too soon to play this sound again
                }
            }

            soundCooldowns[name] = Time.time; // Update last played time

            switch (sound.soundType)
            {
                case SoundType.BGSound:
                    bgAudioSource.clip = sound.clip;
                    bgAudioSource.Play();
                    break;
                case SoundType.MusicSound:
                    musicAudioSource.clip = sound.clip;
                    musicAudioSource.loop = true;
                    musicAudioSource.Play();
                    break;
                case SoundType.VFXSound:
                    PlayFromPoolVFX(sound.clip);
                    break;
                case SoundType.Gunshot:
                    PlayFromPool(sound.clip);
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found!");
        }
    }
    public AudioSource GetAudioSourceForType(SoundType type)
    {
        switch (type)
        {
            case SoundType.VFXSound:
            case SoundType.Gunshot:
                return audioSourcePool[currentSourceIndex];
            case SoundType.BGSound:
                return bgAudioSource;
            case SoundType.MusicSound:
                return musicAudioSource;
            default:
                return null;
        }
    }

    private void PlayFromPool(AudioClip clip)
    {
        if (audioSourcePool.Count == 0) return;

        AudioSource source = audioSourcePool[currentSourceIndex];
        source.clip = clip;
        source.Play();

        // Move to the next source in the pool
        currentSourceIndex = (currentSourceIndex + 1) % poolSize;
    }
    private void PlayFromPoolVFX(AudioClip clip)
    {
        if (VFXaudioSourcePool.Count == 0) return;

        AudioSource source = VFXaudioSourcePool[VFXcurrentSourceIndex];
        source.clip = clip;
        source.Play();

        // Move to the next source in the pool
        VFXcurrentSourceIndex = (VFXcurrentSourceIndex + 1) % poolSize;
    }
}
