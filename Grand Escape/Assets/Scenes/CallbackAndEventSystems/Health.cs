using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class Health : MonoBehaviour
    {

        void Start()
        {
            startingScale = transform.localScale;
        }

        //Define a "template" or signature for a function
        public delegate void OnDeathCallbackDelegate(UnitDeathEventInfo unitDeathInfo);
        static public event OnDeathCallbackDelegate OnDeathListeners;

        [SerializeField] float explosionDelay = 1f;
        [SerializeField] float inflationScale = 5f;

        Vector3 startingScale;
        float delayTimer = 0f;
        bool isClicked = false;

        void Update()
        {
            if (isClicked && delayTimer < explosionDelay)
            {
                delayTimer += Time.deltaTime;
                transform.localScale = startingScale + startingScale * inflationScale * delayTimer / explosionDelay;
            }

            if (isClicked && delayTimer >= explosionDelay)
            {
                isClicked = false;
                Die();
            }
        }

        private void OnMouseDown()
        {
            isClicked = true;
        }

        void Die()
        {
            UnitDeathEventInfo index = new UnitDeathEventInfo();
            index.eventDescription = "Unit " + gameObject.name + " has died.";
            index.unitGO = gameObject;

            EventSystem.Current.FireEvent(index);
        }
    }
}
