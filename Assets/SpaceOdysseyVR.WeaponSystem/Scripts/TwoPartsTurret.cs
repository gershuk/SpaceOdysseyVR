#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    public sealed class TwoPartsTurret : MonoBehaviour
    {
        [SerializeField]
        private static GameObject? _aimPointMarkerPrefab;

        [SerializeField]
        private static GameObject? _ammorPrefab;

        private GameObject _aimPointMarker;

        [SerializeField]
        [Range(0.01f, 10f)]
        private float _cooldown = 0.01f;

        private float _lastShootTime;

        [SerializeField]
        private float _pitchLimit = 90f;

        [SerializeField]
        private Transform _pitchSegment;

        private Quaternion _pitchSegmentStartRotation;

        [SerializeField]
        private float _pitchSpeed = 30f;

        [SerializeField]
        private Transform _shootPoint;

        [SerializeField]
        private float _yawLimit = 90f;

        [SerializeField]
        private Transform _yawSegment;

        private Quaternion _yawSegmentStartRotation;

        [SerializeField]
        private float _yawSpeed = 30f;

        public float RemainingTime => Math.Max(0, _lastShootTime + _cooldown - Time.time);

        public Vector3 TargetPoint { get; set; }

        private void RotateToTarget ()
        {
            float angle;
            Vector3 targetRelative;
            Quaternion targetRotation;
            if (_yawSegment && _yawLimit != 0f)
            {
                targetRelative = _yawSegment.InverseTransformPoint(TargetPoint);
                angle = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
                if (angle >= 180f)
                    angle = 180f - angle;
                if (angle <= -180f)
                    angle = -180f + angle;
                targetRotation = _yawSegment.rotation
                    * Quaternion.Euler(0f,
                                       Mathf.Clamp(angle, -_yawSpeed * Time.deltaTime, _yawSpeed * Time.deltaTime),
                                       0f);
                _yawSegment.rotation = _yawLimit is < 360f and > 0f
                    ? Quaternion.RotateTowards(_yawSegment.parent.rotation * _yawSegmentStartRotation, targetRotation, _yawLimit)
                    : targetRotation;
            }

            if (_pitchSegment && _pitchLimit != 0f)
            {
                targetRelative = _pitchSegment.InverseTransformPoint(TargetPoint);
                angle = -Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;
                if (angle >= 180f)
                    angle = 180f - angle;
                if (angle <= -180f)
                    angle = -180f + angle;
                targetRotation = _pitchSegment.rotation
                    * Quaternion.Euler(Mathf.Clamp(angle, -_pitchSpeed * Time.deltaTime, _pitchSpeed * Time.deltaTime),
                                       0f,
                                       0f);
                _pitchSegment.rotation = _pitchLimit is < 360f and > 0f
                    ? Quaternion.RotateTowards(_pitchSegment.parent.rotation * _pitchSegmentStartRotation,
                                               targetRotation,
                                               _pitchLimit)
                    : targetRotation;
            }
        }

        private void Start ()
        {
            (_yawSegmentStartRotation, _pitchSegmentStartRotation) =
                (_yawSegment.localRotation, _pitchSegment.localRotation);

            _lastShootTime = Time.time;

            if (_ammorPrefab == null)
                _ammorPrefab = Resources.Load<GameObject>(@"Prefabs/Laser2");

            if (_aimPointMarkerPrefab == null)
                _aimPointMarkerPrefab = Resources.Load<GameObject>(@"Prefabs/Aim");

            _aimPointMarker = Instantiate(_aimPointMarkerPrefab, transform);
        }

        private void Update ()
        {
            RotateToTarget();
            UpdateAimPointMarker();
        }

        private void UpdateAimPointMarker ()
        {
            if (Physics.Raycast(_shootPoint.position, _shootPoint.forward, out var hitInfo, 1e6f, ~(1 << 2)))
            {
                _aimPointMarker.SetActive(true);
                _aimPointMarker.transform.position = hitInfo.point;
            }
            else
            {
                _aimPointMarker.SetActive(false);
            }
        }

        public bool Shoot ()
        {
            const float eps = 1e-5f;
            var cond = RemainingTime < eps;
            if (cond)
            {
                _ = Instantiate(_ammorPrefab, _shootPoint.position, _shootPoint.rotation);
                _lastShootTime = Time.time;
            }
            return cond;
        }
    }
}