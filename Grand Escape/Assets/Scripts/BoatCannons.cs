//Author: William Ohldin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCannons : MonoBehaviour
{
    [SerializeField] private AudioClip cannonShot;
    /*[SerializeField] private AudioClip shot2;
    [SerializeField] private AudioClip shot3;
    [SerializeField] private AudioClip shot4;*/
    [SerializeField] private GameObject smokeHolder;
    [SerializeField] private ParticleSystem cannonSmoke;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;

    private AudioSource source;
    private bool soundIsPlaying;
    private float randomTimer;

    void Start()
    {
        source = GetComponent<AudioSource>();
        /*sources[0].clip = shot1;
        sources[1].clip = shot2;
        sources[2].clip = shot3;
        sources[3].clip = shot4;*/
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            soundIsPlaying = false;

            randomTimer = Random.Range(minDelay, maxDelay);

            source.PlayDelayed(randomTimer);
        }

        if (source.isPlaying & !soundIsPlaying)
        {
            Invoke("PlayParticleSystem", randomTimer);
            soundIsPlaying = true;
        }

        /*if (!sources[1].isPlaying)
        {
            float d = Random.Range(minDelay, maxDelay);
            sources[1].PlayDelayed(d);
        }

        if (!sources[2].isPlaying)
        {
            float d = Random.Range(minDelay, maxDelay);
            sources[2].PlayDelayed(d);
        }

        if (!sources[3].isPlaying)
        {
            float d = Random.Range(minDelay, maxDelay);
            sources[3].PlayDelayed(d);
        }*/
    }

    void PlayParticleSystem()
    {
        Instantiate(cannonSmoke, smokeHolder.transform.position, smokeHolder.transform.rotation);
    }
}
