#nullable enable

using System;
using System.Collections;

using Unity.Collections;

using UnityEngine;

namespace SpaceOdysseyVR.HandTeleporter
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class TrajectoryCalculator : MonoBehaviour
    {
        public event Action<Vector3>? SphereReachEndPoint;

        private Transform _transform;
        private LineRenderer _lineRenderer;

        [SerializeField]
        private GameObject _spherePrefab;

        private TeleportSphere? _teleportSphere;

        #region Coroutines

        private Coroutine? _updateTrajectoryCoroutine;
        private Coroutine? _moveSphereToLastPointCoroutine;
        private bool _isTeleporting;

        #endregion Coroutines

        #region Graphical parameters

        [SerializeField]
        private Material _mainMaterial;

        [SerializeField]
        private Color _color = Color.blue;

        [SerializeField]
        [Range(0.05f, 1f)]
        private float _lineWidth = 0.05f;

        #endregion Graphical parameters

        #region Trajectory parameters

        [SerializeField]
        [Range(0f, 10f)]
        private float _forwardSpeed = 0.1f;

        [SerializeField]
        [Range(0f, 10f)]
        private float _gravityAcceleration = 0.0125f;

        [SerializeField]
        [Range(1, 1000)]
        private int _stepsCount = 300;

        [SerializeField]
        [Range(0f, 1f)]
        private float _frictionСoefficient = 0;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float _sphereRadius = 2;

        [SerializeField]
        [Range(0.01f, 2f)]
        private float _segmentTime = 1;

        #endregion Trajectory parameters

        #region Trajectory Data

        private NativeArray<Vector3> _trajectory;
        private int _last;
        private float _trajectoryLength;

        #endregion Trajectory Data

        public Vector3? LastTrajectoryPoint => _trajectory != null ? _trajectory[_last] : null;

        private void Start ()
        {
            _transform = GetComponent<Transform>();

            _mainMaterial.color = _color;

            var sphere = Instantiate(_spherePrefab);
            _teleportSphere = sphere.GetComponent<TeleportSphere>();

            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
            _lineRenderer.material = _mainMaterial;

            _trajectory = new(_stepsCount + 1, Allocator.Persistent);
            _trajectoryLength = 0;
        }

        private void CalcTrajectory ()
        {
            _trajectoryLength = 0;
            _trajectory[0] = transform.position;
            _last = 0;

            for (var i = 0; i < _stepsCount; ++i)
            {
                var newPoint = _trajectory[_last]
                               + (1 - _frictionСoefficient) * _forwardSpeed * _transform.forward
                               + (i + 1) * _gravityAcceleration * Vector3.down;
                _trajectory[++_last] = newPoint;

                _trajectoryLength += Vector3.Distance(_trajectory[_last], _trajectory[_last - 1]);

                if (Physics.CheckSphere(newPoint, _sphereRadius))
                {
                    break;
                }
            }
        }

        private void RedrawTrajectory ()
        {
            _lineRenderer.positionCount = _last + 1;
            _lineRenderer.SetPositions(_trajectory);
        }

        public void EndTeleporting ()
        {
            _isTeleporting = false;
            _lineRenderer.enabled = false;

            if (_updateTrajectoryCoroutine != null)
                StopCoroutine(_updateTrajectoryCoroutine);
            if (_moveSphereToLastPointCoroutine != null)
                StopCoroutine(_moveSphereToLastPointCoroutine);

            _moveSphereToLastPointCoroutine = StartCoroutine(MoveSphereToLastPoint());
        }

        public void StratTeleporting ()
        {
            if (Physics.CheckSphere(_transform.position, _sphereRadius))
                return;

            _lineRenderer.enabled = true;
            _isTeleporting = true;

            if (_teleportSphere != null)
            {
                _teleportSphere.State = TeleportSphereState.Visible;
                _teleportSphere.Size = _sphereRadius * 2;
                _teleportSphere.Material = _mainMaterial;
            }

            if (_updateTrajectoryCoroutine != null)
                StopCoroutine(_updateTrajectoryCoroutine);
            if (_moveSphereToLastPointCoroutine != null)
                StopCoroutine(_moveSphereToLastPointCoroutine);

            _updateTrajectoryCoroutine = StartCoroutine(UpdateTrajectory());
        }

        private IEnumerator UpdateTrajectory ()
        {
            while (_isTeleporting)
            {
                CalcTrajectory();
                RedrawTrajectory();
                if (LastTrajectoryPoint.HasValue && _teleportSphere != null)
                    _teleportSphere.Position = LastTrajectoryPoint.Value;
                yield return null;
            }
        }

        private void OnDestroy ()
        {
            if (_teleportSphere != null)
                Destroy(_teleportSphere.gameObject);

            if (_trajectory.IsCreated)
                _trajectory.Dispose();
        }

        private IEnumerator MoveSphereToLastPoint ()
        {
            const float eps = 1e-5f;
            if (_last == 0 || !LastTrajectoryPoint.HasValue)
            {
                yield break;
            }

            var firstIndex = 0;
            var secondIndex = 1;
            var segmentStartTime = Time.time;

            if (_teleportSphere != null)
            {
                _teleportSphere.Position = _trajectory[0];
                _teleportSphere.State = TeleportSphereState.Moving;
                while (Vector3.Distance(_teleportSphere.Position, LastTrajectoryPoint.Value) > eps)
                {
                    _teleportSphere.Position = Vector3.Lerp(_trajectory[firstIndex],
                                                            _trajectory[secondIndex],
                                                            (Time.time - segmentStartTime) / _segmentTime);

                    if (Vector3.Distance(_teleportSphere.Position, _trajectory[secondIndex]) < eps)
                    {
                        firstIndex++;
                        secondIndex++;
                        segmentStartTime = Time.time;
                    }

                    yield return null;
                }
                _teleportSphere.State = TeleportSphereState.Invisible;
            }

            SphereReachEndPoint?.Invoke(LastTrajectoryPoint.Value);
        }
    }
}