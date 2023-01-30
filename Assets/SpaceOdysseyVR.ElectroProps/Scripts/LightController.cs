#nullable enable

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(Light))]
    public sealed class LightController : AbstractProp
    {
        [SerializeField]
        private Light _light;

        [SerializeField]
        [Range(0.01f, 10f)]
        private float _maxIntensity = 1f;

        private void OnDestroy ()
        {
            if (_powerCore != null)
            {
                _powerCore.OnCorePowerOff -= OnPowerOff;
                _powerCore.OnCorePowerOn -= OnPowerOn;
                _powerCore.OnCoreStartingProgressChange -= OnStartingProgressChange;
            }
        }

        protected override void OnStartingProgressChange (float value)
        {
            base.OnStartingProgressChange(value);
            _light.intensity = _maxIntensity * value;
        }

        protected override void PowerOff ()
        {
            _light.enabled = false;
        }

        protected override void PowerOn () => _light.enabled = true;

        protected override void Start ()
        {
            base.Start();
            _light = GetComponent<Light>();
            _light.intensity = _maxIntensity;
        }
    }
}