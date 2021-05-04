using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class DestroyParticleSystem : MonoBehaviour
    {
        [System.Obsolete]
        private void Awake()
        {
            Destroy(gameObject, GetComponent<ParticleSystem>().duration);
        }
    }
}