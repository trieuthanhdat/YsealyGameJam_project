using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class Sound
{
    [Header("Basic Settings")]
    public string name;
    public AudioClip clip;
    public bool playOnAwake;
    public bool isBackgroundSound = false;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(0f, 2f)]
    public float pitch = 1f;

    [Header("Fade Settings")]
    [Tooltip("Duration of the fade-in effect in seconds")]
    [Range(0f, 60f)]
    public float fadeInDuration = 1f;
    [Tooltip("Duration of the fade-out effect in seconds")]
    [Range(0f, 60f)]
    public float fadeOutDuration = 1f;

    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public bool useFadeInEffect = false;
    [HideInInspector]
    public bool useFadeOutEffect = false;
    [HideInInspector]
    public MonoAudioPlayer player;
}


public class MonoAudioManager : MonoSingleton<MonoAudioManager>
{
    [SerializeField] MonoAudioPlayer audioPlayerPrefabs;
    [SerializeField] Sound[] sounds;

    bool isGradient = false;
    int gradientSoundIndex = -1;
    float time = 0;
    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            //SETUP PLAYER
            s.player = Instantiate(audioPlayerPrefabs, transform);
            s.player.sound = s;
            s.player.fadeInTimer = s.fadeInDuration;
            s.player.fadeOutTimer = s.fadeOutDuration;
            //SETUP SOUND
            s.audioSource = s.player.gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;

            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.pitch = s.pitch;
            if (s.isBackgroundSound)
                s.audioSource.volume = 0;
            else
                s.audioSource.volume = s.volume;
            //DISABLE TO SAVE PERFOMANCE
            ToggleActivationPlayer(false, s.player);
        }

    }
    public void ToggleActivationPlayer(bool isOn, MonoAudioPlayer player)
    {
        player.gameObject.SetActive(isOn);
    }
    public void PlaySound(string name,bool isLoop = false, bool isGradient = false, float delay = 0)
    {
        try
        {

           Sound s = Array.Find(sounds, sound => sound.name == name);
            if(s == null)
            {
                Debug.LogWarning("MONOAUDIOMANAGER:Sound name: "+name +" is missing!!!");
                return;
            }
            s.audioSource.loop = isLoop;
            s.useFadeInEffect = isGradient;
            s.player.PlaySound(delay);
        }catch(Exception ex)
        {
            Debug.Log("MONOAUDIOMANAGER: exception at PlaySound " + ex);
        }
    }

    public void StopSound(string name, bool isGradient = false, float modifiedFadeoutTime = -1)
    {
        try
        {

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound name: " + s.name + " is missing!!!");
                return;
            }
            s.useFadeOutEffect = isGradient;

            if(modifiedFadeoutTime > -1)
                s.fadeOutDuration = modifiedFadeoutTime;
            s.player.StopSound();
        }catch(Exception ex)
        {
            Debug.Log("MONOAUDIOMANAGER: exception at StopSound " + ex);
        }
    }


}
