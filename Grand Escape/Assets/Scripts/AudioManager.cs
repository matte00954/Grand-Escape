//Author: William Örnquist
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    //[Range(0.1f, 2f)]
    //[SerializeField] private float walkSoundFrequency = 0.7f;

    //private PlayerMovement playerMovement;

    //private float walkSoundFrequencyTimer;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.Source = gameObject.AddComponent<AudioSource>();
            sound.Source.clip = sound.Clip;

            sound.Source.volume = sound.Volume;
            sound.Source.pitch = sound.Pitch;
            sound.Source.loop = sound.Loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.Play();
    }

    private void Start()
    {
        Play("DefaultBGM");
    }

    //private void Update()
    //{
    //    if (PlayerMovement.IsMoving && walkSoundFrequencyTimer >= walkSoundFrequency)
    //    {
    //        Play("Walk1");
    //        walkSoundFrequencyTimer = 0f;
    //    }
    //    else
    //        walkSoundFrequencyTimer += Time.deltaTime;
    //}
}