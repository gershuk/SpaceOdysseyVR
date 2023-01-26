using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.StatusLight
{
    public class SetLight : MonoBehaviour
    {
        public Status statusComplete;
        private Light lightComponent;

        //-------------------------------------------------
        void Start ()
        {
            if (lightComponent == null)
            {
                lightComponent = GetComponentInChildren<Light>();
                lightComponent.color = Color.red;
            }

            if (statusComplete == null)
            {
                statusComplete = GetComponent<Status>();
            }
        }


        //-------------------------------------------------
        void Update ()
        {
            lightComponent.color = statusComplete.GetStatusComplete() ? Color.green : Color.red;
        }
    }
}