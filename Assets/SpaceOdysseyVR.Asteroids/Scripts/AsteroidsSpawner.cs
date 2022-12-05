#nullable enable

using System.Collections;
using System.Collections.Generic;

using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.Asteroids
{
    public readonly struct SpawnParameters
    {
        private readonly uint _allDirectAsteroidsCount;
        private readonly uint _partitionDirectAsteroidsCount;
        private readonly float _spawnInterval;
        private readonly float? _timeout;

        public uint AllDirectAsteroidsCount => _allDirectAsteroidsCount;

        public uint PartitionDirectAsteroidsCount => _partitionDirectAsteroidsCount;

        public float SpawnInterval => _spawnInterval;

        public float? Timeout => _timeout;

        public SpawnParameters (float spawnInterval,
                                uint allDirectAsteroidsCount,
                                uint partitionDirectAsteroidsCount,
                                float? timeout = null)
        {
            _spawnInterval = spawnInterval;
            _allDirectAsteroidsCount = allDirectAsteroidsCount;
            _partitionDirectAsteroidsCount = partitionDirectAsteroidsCount;
            _timeout = timeout;
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class AsteroidsSpawner : MonoBehaviour
    {
        private readonly HashSet<Asteroid> _spawnedAsteroids = new();

        [SerializeField]
        private GameObject[] _asteroidPrefabs;

        private BoxCollider _boxCollider;

        [SerializeField]
        private Vector3 _boxSize;

        private Coroutine? _spawningCoroutine = null;

        [SerializeField]
        private Transform _spawnPoint;

        [SerializeField]
        private Transform _transform;

        #region Spawn support positions

        private Vector3 BottomLeftSpawnPoint =>
            StartTransform.position
            + (StartTransform.right * _boxSize.x + StartTransform.up * _boxSize.y) / -2;

        private Vector3 BottomLeftTerminatePoint =>
            StartTransform.position
            + StartTransform.forward * _boxSize.z
            + (StartTransform.right * _boxSize.x + StartTransform.up * _boxSize.y) / -2;

        private Vector3 SpawnStartCenter =>
                    (_spawnPoint == null ? _transform.position : _spawnPoint.position)
            + Vector3.forward * _boxSize.z / 2;

        private Transform StartTransform => _spawnPoint == null ? _transform : _spawnPoint;

        private Vector3 TopRightSpawnPoint =>
            StartTransform.position
            + (StartTransform.right * _boxSize.x + StartTransform.up * _boxSize.y) / 2;

        private Vector3 TopRightTerminatePoint =>
            StartTransform.position
            + StartTransform.forward * _boxSize.z
            + (StartTransform.right * _boxSize.x + StartTransform.up * _boxSize.y) / 2;

        private Vector3 GetRandomEndPoint () =>
            GetRandomSpawnPoint() + StartTransform.forward * _boxSize.z;

        private Vector3 GetRandomSpawnPoint ()
        {
            var bottomLeft = BottomLeftSpawnPoint;
            var topRigth = TopRightSpawnPoint;
            var min = Vector3.Min(bottomLeft, topRigth);
            var max = Vector3.Max(bottomLeft, topRigth);
            return new(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z)
            );
        }

        #endregion Spawn support positions

        private void OnDrawGizmos ()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(SpawnStartCenter, _boxSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(BottomLeftSpawnPoint, 1f);
            Gizmos.DrawWireSphere(TopRightSpawnPoint, 1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(BottomLeftTerminatePoint, 1f);
            Gizmos.DrawWireSphere(TopRightTerminatePoint, 1f);
        }

        private void Start ()
        {
            _transform = transform;
            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;
            _boxCollider.size = _boxSize * 1.5f;
            _boxCollider.center = SpawnStartCenter;
        }

        public IEnumerator SpawningCoroutine (SpawnParameters parameters)
        {
            var time = Time.time;
            var remainsDirectAsteroidsCount = parameters.AllDirectAsteroidsCount;

            while (!(parameters.Timeout.HasValue && Time.time - time < parameters.Timeout)
                    && remainsDirectAsteroidsCount > 0)
            {
                for (var i = 0; i < parameters.PartitionDirectAsteroidsCount; ++i)
                {
                    var prefab = _asteroidPrefabs[UnityEngine.Random.Range(0, _asteroidPrefabs.Length - 1)];
                    var asteroid = Instantiate(prefab).GetComponent<Asteroid>();
                    asteroid.GetComponent<HealthComponent>().OnDeath += () => _spawnedAsteroids.Remove(asteroid);
                    asteroid.StartPosition = GetRandomSpawnPoint();
                    asteroid.EndPosition = GetRandomEndPoint();
                    _spawnedAsteroids.Add(asteroid);
                }

                remainsDirectAsteroidsCount -= parameters.PartitionDirectAsteroidsCount;

                yield return new WaitForSeconds(parameters.SpawnInterval);
            }
            _spawningCoroutine = null;
        }

        [ContextMenu("Start spawning")]
        public void StartSpawning ()
        {
            if (_spawningCoroutine != null)
            {
                Debug.LogError("Spawning already started");
                return;
            }

            _spawningCoroutine = StartCoroutine(SpawningCoroutine(new(3f, 50, 10)));
        }

        [ContextMenu("Stop spawning")]
        public void StopSpawning ()
        {
            if (_spawningCoroutine != null)
            {
                StopCoroutine(_spawningCoroutine);
                _spawningCoroutine = null;
            }
        }
    }
}