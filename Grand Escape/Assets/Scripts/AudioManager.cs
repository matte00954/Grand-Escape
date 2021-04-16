using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds;
    [Range(0.1f, 2f)]
    [SerializeField] float walkSoundFrequency = 0.7f;

    float walkSoundFrequencyTimer;

    void Awake()
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

    private void Update()
    {
        if (FindObjectOfType<PlayerMovement>().IsWalking() && walkSoundFrequencyTimer >= walkSoundFrequency)
        {
            Play("Walk1");
            walkSoundFrequencyTimer = 0f;
        }
        else
            walkSoundFrequencyTimer += Time.deltaTime;
    }
}
