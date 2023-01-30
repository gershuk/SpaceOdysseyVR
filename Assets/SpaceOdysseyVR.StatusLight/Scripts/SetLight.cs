using UnityEngine;

namespace SpaceOdysseyVR.StatusLight
{
    public class SetLight : MonoBehaviour
    {
        private Light lightComponent;
        public Status statusComplete;

        //-------------------------------------------------
        private void Start ()
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
        private void Update ()
        {
            lightComponent.color = statusComplete.IsComplete ? Color.green : Color.red;
        }
    }
}