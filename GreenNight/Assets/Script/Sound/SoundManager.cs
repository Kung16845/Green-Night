using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGSound,
    MusicSound,
    VFXSound
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public SoundType soundType;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound List")]
    public List<Sound> sounds = new List<Sound>();

    private AudioSource bgAudioSource;
    private AudioSource musicAudioSource;
    private AudioSource vfxAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            vfxAudioSource = gameObject.AddComponent<AudioSource>();
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
            switch (sound.soundType)
            {
                case SoundType.BGSound:
                    bgAudioSource.clip = sound.clip;
                    bgAudioSource.loop = true;
                    bgAudioSource.Play();
                    break;
                case SoundType.MusicSound:
                    musicAudioSource.clip = sound.clip;
                    musicAudioSource.loop = false;
                    musicAudioSource.Play();
                    break;
                case SoundType.VFXSound:
                    vfxAudioSource.PlayOneShot(sound.clip);
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{name}' not found!");
        }
    }
}
