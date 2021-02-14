using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private float musicMultiplier;
    private float effectMultiplier;
    private bool mute;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    private void Start()
    {
        Play("Background");
    }
    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + s.name + "not found");
            return;
        }
        s.source.Play();
    }

    public void SetMute(bool isMuted)
    {
        mute = isMuted;
        if (isMuted)
        {
            SetMusicVolume(0);
            SetEffectVolume(0);
        }
        else
        {
            UpdateSoundVolumes();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (mute)
        {
            musicMultiplier = 0;
        }
        else
        {
            musicMultiplier = volume;
        }
    }

    public void SetEffectVolume(float volume)
    {
        if (mute)
        {
            effectMultiplier = 0;
        }
        else
        {
            effectMultiplier = volume;
        }
    }

    public void UpdateSoundVolumes()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * effectMultiplier;
        }

        Sound background = Array.Find(sounds, sound => sound.name == "Background");
        if (background == null)
        {
            Debug.LogWarning("Sound: " + background.name + "not found");
            return;
        }
        background.source.volume = background.volume * musicMultiplier;

    }
}
