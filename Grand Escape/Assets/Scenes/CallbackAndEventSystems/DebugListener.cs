using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class DebugListener : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            EventSystem.Current.RegisterListener<UnitDeathEventInfo>(OnUnitDied);
        }

        void OnUnitDied(UnitDeathEventInfo unitDeathInfo)
        {
            Debug.Log("Died unit name: " + unitDeathInfo.unitGO.name);
        }
    }
}
