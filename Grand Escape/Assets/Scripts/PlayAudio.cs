using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Leo Mendonca Agild leme2980
public class PlayAudio : MonoBehaviour
{
    public AudioClip clip;

    [SerializeField] private AudioSource source;
    [SerializeField] private float delay;
    private bool played;

    // Start is called before the first frame update
    void Start()
    {
        played = false;
        source = GetComponent<AudioSource>();
        source.clip = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (played&&!source.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger!!");

        if (other.CompareTag("Player")&&!source.isPlaying)
        {
            source.PlayDelayed(delay);
            played = true;
         
        }
    }
}
