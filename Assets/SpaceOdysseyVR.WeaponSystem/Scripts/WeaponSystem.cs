#nullable enable

using SpaceOdysseyVR.ElectroProps;
using SpaceOdysseyVR.WeaponCharger;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    public class WeaponSystem : MonoBehaviour
    {
        private bool _active;

        [SerializeField]
        private Charger _charger;

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private Transform? _pointer;

        private PowerCore _powerCore;

        [SerializeField]
        [Range(0.01f, 10f)]
        private float _sphereCastRad = 2;

        [SerializeField]
        private TwoPartsTurret[] _turrets;

        private void OnDisable ()
        {
            foreach (var turret in _turrets)
            {
                if (turret != null)
                    turret.gameObject.SetActive(false);
            }
        }

        private void OnEnable ()
        {
            foreach (var turret in _turrets)
            {
                turret.gameObject.SetActive(true);
            }
        }

        private void OnPowerOff () => _active = false;

        private void OnPowerOn () => _active = true;

        private void Start ()
        {
            if (_charger == null)
                _charger = FindObjectOfType<Charger>();

            _active = true;
            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _powerCore.OnCorePowerOff += OnPowerOff;
            _powerCore.OnCorePowerOn += OnPowerOn;

            _active = _powerCore.CoreState is not CoreState.None and not CoreState.Stopped;

            if (_pointer == null)
                _pointer = transform;

            foreach (var turret in _turrets)
            {
                turret.TargetPoint = turret.transform.forward * 1 + turret.transform.position;
            }
        }

        private void Update ()
        {
            if (_active)
            {
                var point = Physics.SphereCast(_pointer!.position,
                                               _sphereCastRad,
                                               _pointer.forward,
                                               out var raycastHit,
                                               _maxDistance,
                                               ~(1 << 2))
                    ? raycastHit.transform.position
                    : _pointer.position + _pointer.forward * _maxDistance;
                foreach (var turret in _turrets)
                {
                    turret.TargetPoint = point;
                }
            }
        }

        [ContextMenu(nameof(Shoot))]
        public void Shoot ()
        {
            if (_active && _charger.IsCharged())
            {
                _charger.DischargePowerUnit();
                foreach (var turret in _turrets)
                {
                    _ = turret.Shoot();
                }
            }
        }
    }
}