#nullable enable

using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponSystem
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
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

        private void AddForce () => _rigidbody.AddRelativeForce(new Vector3(0, 0, _forceModule), ForceMode.Impulse);

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
            Invoke(nameof(AddForce), 0.3f);

            Destroy(gameObject, _lifeTime);
        }
    }
}