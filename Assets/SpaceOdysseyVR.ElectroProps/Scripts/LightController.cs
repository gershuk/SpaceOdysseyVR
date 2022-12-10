#nullable enable

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(Light))]
    public sealed class LightController : MonoBehaviour
    {
        private Light _light;

        [SerializeField]
        [Range(0.01f, 10f)]
        private float _maxIntensity = 1f;

        [SerializeField]
        private PowerCore? _powerCore;

        private void OnDestroy ()
        {
            if (_powerCore != null)
            {
                _powerCore.OnPowerOff -= OnPowerOff;
                _powerCore.OnPowerOn -= OnPowerOn;
                _powerCore.OnStartingProgressChange -= OnStartingProgressChange;
            }
        }

        private void OnPowerOff () => _light.enabled = false;

        private void OnPowerOn () => _light.enabled = true;

        private void OnStartingProgressChange (float value) =>
            _light.intensity = _maxIntensity * value;

        private void Start ()
        {
            _light = GetComponent<Light>();
            _light.intensity = _maxIntensity;

            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _powerCore.OnPowerOff += OnPowerOff;
            _powerCore.OnPowerOn += OnPowerOn;
            _powerCore.OnStartingProgressChange += OnStartingProgressChange;

            if (_powerCore.CoreState is not CoreState.Working)
                OnPowerOff();
        }
    }
}