using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{
    [System.Obsolete]
    private void Awake() => Destroy(gameObject, GetComponent<ParticleSystem>().duration);
}