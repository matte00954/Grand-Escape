using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class ParticleListener : MonoBehaviour
    {
        void Start()
        {
            EventSystem.Current.RegisterListener<UnitDeathEventInfo>(OnUnitDie);
        }

        [SerializeField] ParticleSystem explosionEffect;

        void OnUnitDie(UnitDeathEventInfo unitDeathInfo)
        {
            ParticleSystem ps = Instantiate(explosionEffect);
            ps.transform.position = unitDeathInfo.unitGO.transform.position;
        }
    }
}
