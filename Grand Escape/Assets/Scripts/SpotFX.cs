using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotFX : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private AudioSource source;
    public float minDelay = 0.5f;
    public float maxDelay = 2f;
    public float minVol = 0.5f;
    public float maxVol = 1.5f;
    public float minPitch = 1.0f;
    public float maxPitch = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;

        source.time = Random.Range(0, clip.length);
    }

    void OnTriggerStay()
    {
        float delay = Random.Range(minDelay, maxDelay);     //randomize delay
        float volume = Random.Range(minVol, maxVol);        //randomize volume
        float pitch = Random.Range(minPitch, maxPitch);     //randomize pitch

        if (!source.isPlaying)
        {
            source.volume = 0;     //set volume to 0 before fade-in
            source.pitch = pitch;     //set new randomized pitch
            source.Play();  //play clip after delay
            StartCoroutine(FadeIn(source, delay, volume)); //start fade-in (lasting as long as randomized 'delay') with randomized volume as target
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeOut(source, 2, 0));
        }
    }

    public static IEnumerator FadeIn(AudioSource source, float fadeTime, float finalVolume)
    {
        float startTime = 0;
        float startVolume = 0;

        while (startTime < fadeTime)
        {
            startTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, finalVolume, startTime / fadeTime);
            yield return null;
        }
        yield break;
    }

    public static IEnumerator FadeOut(AudioSource source, float fadeTime, float finalVolume)
    {
        float startTime = 0;
        float startVolume = source.volume;

        while (startTime < fadeTime)
        {
            startTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, finalVolume, startTime / fadeTime);
            yield return null;
        }
        source.Stop();

        yield break;
    }
}
