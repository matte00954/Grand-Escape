using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] AudioClip clip;

    [SerializeField] string name;

    [Range(0f, 1f)]
    [SerializeField] float volume;
    [Range(.1f, 3f)]
    [SerializeField] float pitch;
    [SerializeField] bool loop;

    AudioSource source;


    public AudioClip Clip
    {
        get { return clip; }
        set { clip = value; }
    }

    public AudioSource Source
    {
        get { return source; }
        set { source = value; }
    }

    public float Volume
    {
        get { return volume; }
        set { volume = value; }
    }

    public float Pitch
    {
        get { return pitch; }
        set { pitch = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public bool Loop
    {
        get { return loop; }
        set { loop = value; }
    }
}
