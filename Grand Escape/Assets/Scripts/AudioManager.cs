//Author: William Örnquist
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
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
        Debug.Log("'AudioManager.Play' trying to find: " + name);
        Sound sound = Array.Find(sounds, sound => sound.Name == name);
        if (sound != null)
        {
            try
            {
                sound.Source.pitch = sound.Pitch; //Pitch is set here in the case it must be randomly set on play.
                sound.Source.Play();
            }
            catch { Debug.LogWarning("Null reference detected"); };
        }
        else
            Debug.Log("Did not find soundname");
    }

    private void Start() => Play("DefaultBGM");
}