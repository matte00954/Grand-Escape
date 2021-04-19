using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public abstract class EventInfo
    {
        public string eventDescription;
    }

    public class DebugEventInfo : EventInfo
    {
        public int verbosityLevel;
    }

    public class UnitDeathEventInfo : EventInfo
    {
        public GameObject unitGO;
    }
}