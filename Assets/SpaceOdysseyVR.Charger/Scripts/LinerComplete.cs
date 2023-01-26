using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.Charger
{
    public class LinerComplete : MonoBehaviour
    {
        private SpaceOdysseyVR.StatusLight.StatusComplete statusCompleteLight;
        private Valve.VR.InteractionSystem.LinearMapping linearMapping;
        private StatusComplete statusComplete;
        // Start is called before the first frame update
        void Start ()
        {
            statusCompleteLight = GetComponent<SpaceOdysseyVR.StatusLight.StatusComplete>();
            linearMapping = GetComponent<Valve.VR.InteractionSystem.LinearMapping>();
            statusComplete = GetComponent<StatusComplete>();
        }

        // Update is called once per frame
        void Update ()
        {
            var isComplete = linearMapping.value == 1;
            statusCompleteLight.SetStatusComplete(isComplete);
            statusComplete.SetStatusComplete(isComplete);
        }
    }
}