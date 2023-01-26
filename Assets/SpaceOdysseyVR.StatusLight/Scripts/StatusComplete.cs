using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.StatusLight
{
    public class StatusComplete : MonoBehaviour
    {
        private bool isComplete = false;
        // Start is called before the first frame update
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        public bool GetStatusComplete ()
        {
            return isComplete;
        }

        public void SetStatusComplete (bool newStatus)
        {
            isComplete = newStatus;
        }
    }
}