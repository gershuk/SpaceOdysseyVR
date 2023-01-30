using UnityEngine;

namespace SpaceOdysseyVR.WeaponCharger
{
    public class LinerComplete : MonoBehaviour
    {
        private Valve.VR.InteractionSystem.LinearMapping linearMapping;
        private StatusComplete statusComplete;
        private SpaceOdysseyVR.StatusLight.Status statusCompleteLight;

        // Start is called before the first frame update
        private void Start ()
        {
            statusCompleteLight = GetComponent<SpaceOdysseyVR.StatusLight.Status>();
            linearMapping = GetComponent<Valve.VR.InteractionSystem.LinearMapping>();
            statusComplete = GetComponent<StatusComplete>();
        }

        // Update is called once per frame
        private void Update ()
        {
            var isComplete = linearMapping.value == 1;
            statusCompleteLight.IsComplete = isComplete;
            statusComplete.SetStatusComplete(isComplete);
        }
    }
}