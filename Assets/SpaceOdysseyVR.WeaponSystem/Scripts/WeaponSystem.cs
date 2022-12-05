#nullable enable

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField]
        private float _maxDistance;

        [SerializeField]
        private Transform? _pointer;

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

        private void Start ()
        {
            if (_pointer == null)
                _pointer = transform;
        }

        private void Update ()
        {
            var point = Physics.Raycast(_pointer!.position, _pointer.forward, out var raycastHit, _maxDistance, ~(1 << 2))
                ? raycastHit.point
                : _pointer.forward * _maxDistance;
            foreach (var turret in _turrets)
            {
                turret.TargetPoint = point;
            }
        }

        [ContextMenu(nameof(Shoot))]
        public void Shoot ()
        {
            foreach (var turret in _turrets)
            {
                _ = turret.Shoot();
            }
        }
    }
}