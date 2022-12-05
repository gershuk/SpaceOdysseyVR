#nullable enable

using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    [RequireComponent(typeof(TrailRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public sealed class LaserBullet : MonoBehaviour
    {
        private static Material? _material;

        [SerializeField]
        private uint _damage = 100;

        [SerializeField]
        [Range(0.1f, 1000f)]
        private float _forceModule = 200;

        [SerializeField]
        [Range(2f, 10f)]
        private float _lifeTime = 6f;

        private Rigidbody _rigidbody;

        [SerializeField]
        [Range(0f, 1f)]
        private float _size = 0.5f;

        private SphereCollider _sphereCollider;

        [SerializeField]
        [Range(0.01f, 1f)]
        private float _trailLifeTime = 0.2f;

        private TrailRenderer _trailRenderer;

        private void OnCollisionEnter (Collision collision)
        {
            var healthComponent = collision.transform.GetComponent<HealthComponent>();
            if (healthComponent)
                healthComponent.TakeDamage(_damage);
            Destroy(gameObject);
        }

        private void Start ()
        {
            if (_material == null)
                _material = Resources.Load<Material>(@"Materials/Laser");

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = false;
            _rigidbody.AddRelativeForce(new Vector3(0, 0, _forceModule), ForceMode.Impulse);

            _sphereCollider = GetComponent<SphereCollider>();
            _sphereCollider.radius = _size;
            _sphereCollider.isTrigger = false;

            _trailRenderer = GetComponent<TrailRenderer>();
            _trailRenderer.material = _material;
            _trailRenderer.startWidth = _size;
            _trailRenderer.endWidth = _size;
            _trailRenderer.time = _trailLifeTime;

            Destroy(gameObject, _lifeTime);
        }
    }
}