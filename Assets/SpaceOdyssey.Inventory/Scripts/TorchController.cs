using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace SpaceOdysseyVR.Inventory
{
    public class TorchController : MonoBehaviour
    {
        public float intensityLight = 2.5F;

        void Update ()
        {
            GetComponent<Light>().intensity = intensityLight;
        }
    }
}
