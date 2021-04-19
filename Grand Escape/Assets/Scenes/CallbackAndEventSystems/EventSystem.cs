using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class EventSystem : MonoBehaviour
    {
        void OnEnable()
        {
            _Current = this;
        }

        static private EventSystem _Current;
        static public EventSystem Current
        {
            get
            {
                if (_Current == null)
                    _Current = FindObjectOfType<EventSystem>();

                return _Current;
            }
        }

        delegate void EventListener(EventInfo eventInfo);
        Dictionary<System.Type, List<EventListener>> eventListeners;

        public void RegisterListener<T>(System.Action<T> listener) where T : EventInfo
        {
            System.Type eventType = typeof(T);
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<System.Type, List<EventListener>>();
            }

            if (eventListeners.ContainsKey(typeof(T)) == false || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new List<EventListener>();
            }


            // Wraps type convertion around the event listener. 
            EventListener wrapper = (eventInfo) => { listener((T)eventInfo); };

            eventListeners[eventType].Add(wrapper);
        }

        public void UnregisterListener<T>(System.Action<T> listener) where T : EventInfo
        {
            //TODO
        }

        public void FireEvent(EventInfo eventInfo)
        {
            System.Type trueEventInfoClass = eventInfo.GetType();
            if (eventListeners == null || eventListeners[trueEventInfoClass] == null)
            {
                //No one is listening, we are done.
                return;
            }

            foreach (EventListener listener in eventListeners[trueEventInfoClass])
            {
                listener(eventInfo);
            }
        }
    }
}
