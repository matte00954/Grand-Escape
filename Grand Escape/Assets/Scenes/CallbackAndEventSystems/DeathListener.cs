using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class DeathListener : MonoBehaviour
    {
        void Start()
        {
            EventSystem.Current.RegisterListener<UnitDeathEventInfo>(OnUnitDied);
        }

        void OnUnitDied(UnitDeathEventInfo unitDeathInfo)
        {
            Destroy(unitDeathInfo.unitGO.gameObject);
        }
    }
}