using UnityEngine;

public class DestroySoundSource : MonoBehaviour
{
    [System.Obsolete]
    private void Awake() => Destroy(gameObject, GetComponent<AudioSource>().clip.length);
}