using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class SoundListener : MonoBehaviour
    {
        [SerializeField] AudioClip audioClip;
        AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            EventSystem.Current.RegisterListener<UnitDeathEventInfo>(OnUnitDied);
        }

        void OnUnitDied(UnitDeathEventInfo unitDeathInfo)
        {
            //audioSource.PlayOneShot(audioClip);
            if (!audioSource.isPlaying)
                audioSource.Play();
            else if (audioSource.time > (audioSource.clip.length / 3) * 2) //Checks if the currently played sound has passed 2/3 of the audioclip.
                audioSource.PlayOneShot(audioClip);
        }
    }
}
