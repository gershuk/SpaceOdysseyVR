using SpaceOdysseyVR.AsteroidRadarSystem;
using SpaceOdysseyVR.ElectroProps;

using TMPro;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class PropsStatusUI : MonoBehaviour
    {
        private const string _offlineText = "Offline";

        private const string _onlineText = "Online";

        private readonly Color _offlineColor = Color.red;
        private readonly Color _onlineColor = Color.blue;

        [SerializeField]
        private TextMeshProUGUI _lampStatus;

        [SerializeField]
        private LightController[] _lightControllers;

        [SerializeField]
        private PowerCore _powerCore;

        [SerializeField]
        private AsteroidRadar _radar;

        [SerializeField]
        private TextMeshProUGUI _radarStatus;

        [SerializeField]
        private TextMeshProUGUI _reactorStatus;

        private void Init ()
        {
            if (_powerCore == null)
            {
                _powerCore = FindObjectOfType<PowerCore>();
            }

            if (_radar == null)
            {
                _radar = FindObjectOfType<AsteroidRadar>();
            }

            if (_lightControllers == null | _lightControllers.Length == 0)
            {
                _lightControllers = FindObjectsOfType<LightController>();
            }

            _powerCore.OnCorePowerOff += () => SetReactorStatus(false);
            _powerCore.OnCorePowerOn += () => SetReactorStatus(true);
            SetReactorStatus(_powerCore.CoreState is CoreState.Working or CoreState.Working);

            _radar.PowerStatusChanged += SetRadarStatus;
            SetRadarStatus(_radar.IsAlive && _radar.IsPowered);

            SetLampStatus();

            foreach (var controller in _lightControllers)
            {
                controller.PowerStatusChanged += (b) => SetLampStatus();
            }
        }

        private void SetLampStatus ()
        {
            var maxCount = _lightControllers.Length;
            var count = 0;
            foreach (var lightController in _lightControllers)
            {
                if (lightController.IsAlive && lightController.IsPowered)
                {
                    count++;
                }
            }

            (_lampStatus.text, _lampStatus.color) =
            ($"{count}/{maxCount}", count == maxCount ? _onlineColor : _offlineColor);
        }

        private void SetRadarStatus (bool isActive) =>
            (_radarStatus.text, _radarStatus.color) = isActive
                                                    ? (_onlineText, _onlineColor)
                                                    : (_offlineText, _offlineColor);

        private void SetReactorStatus (bool isActive) =>
            (_reactorStatus.text, _reactorStatus.color) = isActive
                                                        ? (_onlineText, _onlineColor)
                                                        : (_offlineText, _offlineColor);

        private void Start ()
        {
            Invoke(nameof(Init), 0.001f);
        }
    }
}