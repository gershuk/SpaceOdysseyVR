using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

using VolumetricLines;

namespace SpaceOdysseyVR.PlayerTools
{
    public sealed class HealRay : MonoBehaviour
    {
        [Range(0f, 10000f)]
        [SerializeField]
        private float _healPerTick = 1000f;

        [Range(0.1f, 1000f)]
        [SerializeField]
        private uint _maxDistance;

        private Transform _pointer;

        [SerializeField]
        private VolumetricLineBehavior _volumetricLineBehavior;

        private void Start ()
        {
            if (_pointer == null)
                _pointer = transform;
            if (_volumetricLineBehavior)
                _volumetricLineBehavior = GetComponent<VolumetricLineBehavior>();
        }

        private void Update ()
        {
            var distance = Physics.Raycast(_pointer!.position, _pointer.forward, out var raycastHit, _maxDistance, ~(1 << 2))
                ? raycastHit.distance
                : _maxDistance;
            _volumetricLineBehavior.EndPos = Vector3.forward * distance;

            var healthComponent = raycastHit.transform.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.TakeHeal(Time.deltaTime * _healPerTick);
            }
        }
    }
}