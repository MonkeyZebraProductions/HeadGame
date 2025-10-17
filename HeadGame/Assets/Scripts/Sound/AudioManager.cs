using UnityEngine;
using System;
using Unity.Mathematics;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float effectsVolume;
    
    [Range(0f, 1f)]
    [SerializeField] float musicVolume;

    public Sound[] sounds;
    public string WrongString = "Wrong ";
    public int MaxWrongIndex = 7;
    public string HurryUpString = "Hurry Up";
    public int MaxHurryIndex = 5;
    public string DontTouchString = "Don't Touch ";
    public int MaxTouchIndex = 6;
    public string PainString = "Pain ";
    public int MaxPainIndex = 7;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            if (s.isMusic)
                s.source.volume = musicVolume;
            else
                s.source.volume = effectsVolume;

            s.source.loop = s.loop;
            s.source.mute = s.mute;
            s.source.ignoreListenerPause = s.ignorePause;

            if (s.threeD)
                s.source.spatialBlend = 1f;
            else
                s.source.spatialBlend = 0f;
        }
    }

    public void SetEffectsVolume(float newVolume)
    {
        effectsVolume = newVolume;

        foreach (Sound s in sounds)
        {
            if (!s.isMusic) s.source.volume = newVolume;
        }
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;

        foreach (Sound s in sounds)
        {
            if (s.isMusic) s.source.volume = newVolume;
        }
    }

    public float GetEffectsVolume()
    {
        return effectsVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float Length(string name)
    {
        if (name == "") return 0f;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return 0f;
        }
        
        return s.source.clip.length;
    }

    public bool IsPlaying(string name)
    {
        if (name == "") return false;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        
        return s.source.isPlaying;
    }

    public void Play(string name)
    {
        if (name == "") return;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Play();
    }

    public void PlayIfNotPlaying(string name)
    {
        if (name == "") return;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        if (!s.source.isPlaying) s.source.Play();
    }

    public void PlayRandomPain()
    {
        int randomIndex = UnityEngine.Random.Range(1, MaxPainIndex);

        Sound s = Array.Find(sounds, sound => sound.clipName == PainString + randomIndex);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();

    }

    public void PlayRandomWrong()
    {
        int randomIndex = UnityEngine.Random.Range(1, MaxWrongIndex);

        Sound s = Array.Find(sounds, sound => sound.clipName == WrongString + randomIndex);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();

    }

    public void PlayRandomDontTouch()
    {
        int randomIndex = UnityEngine.Random.Range(1, MaxTouchIndex);

        Sound s = Array.Find(sounds, sound => sound.clipName == DontTouchString + randomIndex);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();

    }

    public void PlayRandomHurryUp()
    {
        int randomIndex = UnityEngine.Random.Range(1, MaxHurryIndex);

        Sound s = Array.Find(sounds, sound => sound.clipName == HurryUpString + randomIndex);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();

    }

    public void Play3D(string name, Vector3 position)
    {
        if (name == "") return;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        AudioSource.PlayClipAtPoint(s.clip, position, effectsVolume);
    }

    public void Stop(string name)
    {
        if (name == "") return;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);
        
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }
}