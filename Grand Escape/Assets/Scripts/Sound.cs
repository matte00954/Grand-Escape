//Author: William Örnquist
using UnityEngine;

[System.Serializable]
public class Sound
{
    [Header("Main attributes")]
    [SerializeField] private AudioClip clip;
    [SerializeField] private string name;
    [Range(0f, 1f), SerializeField] private float volume = 1f;
    [Range(.1f, 3f), SerializeField] private float pitchValue = 1f;
    [SerializeField, Range(0f, 3f), Tooltip("Amount of variation in pitch.")] private float pitchRange = 0f;
    [SerializeField] private bool loop = false;

    [Header("3D instance attributes")]
    [Min(0f), SerializeField] private float minDistance;
    [Min(0f), SerializeField] private float maxDistance;

    private AudioSource source;

    public AudioSource Source { get => source; set { source = value; } }
    public AudioClip Clip => clip;
    public string Name => name;
    public float Volume => volume;

    /// <summary>
    /// If the sound's pitchRange is greater than 0:
    /// Getting the pitch will generate a random value depending on the assigned
    /// pitchRange to create variation for repetitive sounds.
    /// </summary>
    public float Pitch 
    {
        get
        {
            if (pitchRange != 0f)
            {
                float pitchDifference = Random.Range(-pitchRange, pitchRange);
                float newPitch = pitchValue + pitchDifference;
                if (newPitch < 0f)
                    return 0f;
                else if (newPitch > 3f)
                    return 3f;
                else return newPitch;
            }
            else return pitchValue;
        }
        private set { pitchValue = value; }
    }
    public bool Loop => loop;
    public float MinDistance => minDistance;
    public float MaxDistance => maxDistance;
}
