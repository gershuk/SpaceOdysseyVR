#nullable enable

using SpaceOdysseyVR.ElectroProps;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    public class WeaponSystem : MonoBehaviour
    {
        private bool _active;

        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private Transform? _pointer;

        private PowerCore _powerCore;

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
            _active = true;
            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _powerCore.OnPowerOff += OnPowerOff;
            _powerCore.OnPowerOn += OnPowerOn;

            if (_pointer == null)
                _pointer = transform;
        }

        private void Update ()
        {
            if (_active)
            {
                var point = Physics.Raycast(_pointer!.position, _pointer.forward, out var raycastHit, _maxDistance, ~(1 << 2))
                    ? raycastHit.point
                    : _pointer.forward * _maxDistance;
                foreach (var turret in _turrets)
                {
                    turret.TargetPoint = point;
                }
            }
        }

        [ContextMenu(nameof(Shoot))]
        public void Shoot ()
        {
            if (_active)
            {
                foreach (var turret in _turrets)
                {
                    _ = turret.Shoot();
                }
            }
        }
    }
}