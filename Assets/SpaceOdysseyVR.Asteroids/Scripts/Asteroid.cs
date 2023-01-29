#nullable enable

using System;

using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.Asteroids
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HealthComponent))]
    public sealed class Asteroid : MonoBehaviour
    {
        [SerializeField]
        private static GameObject _explosionPrefab;

        [SerializeField]
        [Range(ExplosionController.MinStrenght, ExplosionController.MinStrenght)]
        private float _strength = 1;

        private Transform _transform;
        #region Trajectory parameters

        [SerializeField]
        [Range(0f, 100f)]
        private float _forceModule = 10;

        [SerializeField]
        [Range(-10f, 10f)]
        private float _maxTorque = 10f;

        [SerializeField]
        [Range(-10f, 10f)]
        private float _minTorque = -10f;

        [SerializeField]
        public Vector3 EndPosition { get; set; }

        [SerializeField]
        public Vector3 StartPosition { get; set; }

        #endregion Trajectory parameters

        #region Component cache

        private HealthComponent _healthComponent;
        private Rigidbody _rigidbody;

        #endregion Component cache

        #region Damage parameters

        [SerializeField]
        [Range(0, 100)]
        private uint _damge = 100;

        [SerializeField]
        [Range(0, 10)]
        private float _explosionStrength = 5;

        #endregion Damage parameters

        public Vector3 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        public Transform Transform
        {
            get => _transform;
            private set => _transform = value;
        }

        private void OnCollisionEnter (Collision collision)
        {
            var healthComponent = collision.transform.GetComponent<HealthComponent>();
            var asteroid = collision.transform.GetComponent<Asteroid>();
            healthComponent?.TakeDamage(asteroid == null ? _damge : _damge / 10);
        }

        private void OnDeath ()
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity)
                .GetComponent<ExplosionController>()
                .Strength = _strength;
            Destroy(gameObject);
        }

        private void OnTriggerExit (Collider other)
        {
            if (other.GetComponent<AsteroidsSpawner>())
                _healthComponent.Health = 0;
        }

        private void Start ()
        {
            Transform = transform;

            _explosionPrefab = _explosionPrefab == null ? Resources.Load<GameObject>(@"Prefabs/Explosion") : _explosionPrefab;

            if (_minTorque > _maxTorque)
                throw new ArgumentException("Min torque should be less then max torque");
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.position = StartPosition;
            _rigidbody.AddForce(Vector3.Normalize(EndPosition - StartPosition) * _forceModule, ForceMode.Impulse);

            _rigidbody.AddTorque(new(UnityEngine.Random.Range(_minTorque, _maxTorque),
                                     UnityEngine.Random.Range(_minTorque, _maxTorque),
                                     UnityEngine.Random.Range(_minTorque, _maxTorque)),
                                 ForceMode.Impulse);

            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnDeath += OnDeath;
        }
    }
}