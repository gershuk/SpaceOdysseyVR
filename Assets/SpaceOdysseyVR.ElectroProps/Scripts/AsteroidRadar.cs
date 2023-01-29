#nullable enable

using System.Collections.Generic;

using SpaceOdysseyVR.Asteroids;
using SpaceOdysseyVR.ElectroProps;

using UnityEngine;

namespace SpaceOdysseyVR.AsteroidRadar
{
    [RequireComponent(typeof(Canvas))]
    public sealed class AsteroidRadar : MonoBehaviour
    {
        private static GameObject? _pointPrefab;

        private readonly Dictionary<Asteroid, RadarPointController> _pointsControllers = new();

        private AsteroidsSpawner[] _asteroidsSpawners;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        [Range(1f, 100f)]
        private float _pointSize = 1;

        private PowerCore _powerCore;

        [SerializeField]
        private Transform _radarTransform;

        private Transform _transform;

        [SerializeField]
        [Range(0.0001f, 100f)]
        private float _zoomScale = 1;

        private void AddAsteroidPoint (Asteroid asteroid)
        {
            var pointController = Instantiate(_pointPrefab, _transform)!.GetComponent<RadarPointController>();
            pointController.Size = _pointSize;
            _pointsControllers.Add(asteroid, pointController);
        }

        private void DeleteAsteriodPoint (Asteroid asteroid)
        {
            _pointsControllers.Remove(asteroid, out var pointController);
            Destroy(pointController.gameObject);
        }

        private void OnDestroy ()
        {
            foreach (var spawner in _asteroidsSpawners)
            {
                spawner.OnAsteroidSpawned -= AddAsteroidPoint;
                spawner.OnAsteroidDestroyed -= DeleteAsteriodPoint;
            }
        }

        private void OnGUI ()
        {
            foreach (var pair in _pointsControllers)
            {
                if (pair.Key != null)
                {
                    if (pair.Key.Transform == null)
                        continue;
                    var position = (pair.Key.Transform.position - _radarTransform.position) * _zoomScale;
                    position = Quaternion.Euler(0, _radarTransform.eulerAngles.y, 0) * position;
                    pair.Value.Position = new(position.x, position.z, 0);
                }
            }
        }

        private void OnPowerOff () => _canvas.enabled = false;

        private void OnPowerOn () => _canvas.enabled = true;

        private void Start ()
        {
            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _powerCore.OnPowerOff += OnPowerOff;
            _powerCore.OnPowerOn += OnPowerOn;

            _transform = transform;

            if (_pointPrefab == null)
                _pointPrefab = Resources.Load<GameObject>(@"Prefabs/AsteroidPoint");
            _canvas = _canvas == null ? GetComponent<Canvas>() : _canvas;

            _radarTransform = _radarTransform == null ? _transform : _radarTransform;

            _asteroidsSpawners = FindObjectsOfType<AsteroidsSpawner>();

            foreach (var spawner in _asteroidsSpawners)
            {
                spawner.OnAsteroidSpawned += AddAsteroidPoint;
                spawner.OnAsteroidDestroyed += DeleteAsteriodPoint;
                foreach (var spawnedAsteroid in spawner.SpawnedAsteroids)
                {
                    AddAsteroidPoint(spawnedAsteroid);
                }
            }
        }
    }
}