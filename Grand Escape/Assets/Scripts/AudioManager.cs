//Author: William Örnquist
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject soundSourcePrefab;
    [SerializeField] private Sound[] sounds, enemySounds;

    private void Awake()
    {
        SetupSounds(sounds);
        SetupSounds(enemySounds);
    }

    //Setups the AudioSource for every sound instance registered in the array from the inspector.
    private void SetupSounds(Sound[] soundArray) 
    {
        foreach (Sound sound in soundArray)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;
            sound.Source.volume = sound.Volume;
            sound.Source.loop = sound.Loop;
            sound.Source.minDistance = sound.MinDistance;
            sound.Source.maxDistance = sound.MaxDistance;
        }
    }

    /// <summary>Plays a sound in 2D with the matching name.</summary>
    /// <param name="name">The custom name assigned to the desired audio clip. (Check the AudioManager prefab)</param>
    public void Play(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null)
        {
            sound.Source.pitch = sound.Pitch; //Pitch is set here in the case it must be randomly set on play.
            sound.Source.Play();
        }
        else
            Debug.Log("Did not find soundname");
    }

    /// <summary>Instatiates a temporary 3D sound object at a set point in space.</summary>
    /// <param name="name">The custom name assigned to the desired audio clip. (Check the AudioManager prefab)</param>
    /// <param name="point">The point in 3D space the sound will originate from.</param>
    public void Play(string name, Vector3 point)
    {
        Sound sound = GetSound(name);
        if (sound == null)
            return;

        Setup3DInstance(sound);
        Instantiate(soundSourcePrefab, point, Quaternion.identity);
    }

    /// <summary>Instatiates a temporary 3D sound object as child to a parent transform.</summary>
    /// <param name="name">The custom name assigned to the desired audio clip. (Check the AudioManager prefab)</param>
    /// <param name="parent">The parent object's transform.</param>
    public void Play(string name, Transform parent)
    {
        Sound sound = GetSound(name);
        if (sound == null)
            return;

        Setup3DInstance(sound);
        Instantiate(soundSourcePrefab, parent);
    }

    private Sound GetSound(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.Name == name);
        if (sound != null)
            return sound;
        else sound = Array.Find(enemySounds, sound => sound.Name == name);
        if (sound != null)
            return sound;
        else
        {
            Debug.LogError("AudioManager.GetSound(string), sound name does not exist: " + name);
            return null; 
        }
    }

    private void Setup3DInstance(Sound sound)
    {
        GameObject soundObject = new GameObject();
        soundObject.AddComponent<AudioSource>();
        soundObject.AddComponent<DestroySoundSource>();
        //audioSource.spatialBlend = 1f;
    }

    private void Start()
    {
        Play("DefaultBGM");
    }
}