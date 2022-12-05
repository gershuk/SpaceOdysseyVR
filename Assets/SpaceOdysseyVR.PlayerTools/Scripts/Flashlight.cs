using UnityEngine;

namespace SpaceOdysseyVR.PlayerTools
{
    public sealed class Flashlight : MonoBehaviour
    {
        [SerializeField]
        private Light _light;

        public float Intensity
        {
            get => _light.intensity;
            set => _light.intensity = value;
        }

        public void Off () => _light.enabled = false;

        public void On () => _light.enabled = true;
    }
}