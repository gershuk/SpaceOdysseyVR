#nullable enable

using System;

using SpaceOdysseyVR.Asteroids;
using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(HealthComponent))]
    [RequireComponent(typeof(BoxCollider))]
    public class SpaceShipHull : MonoBehaviour
    {
        private HealthComponent _healthComponent;

        private PowerCore _powerCore;

        public event Action? OnShipDeath;

        private void OnCollisionEnter (Collision collision)
        {
            var healthComponent = collision.transform.GetComponent<HealthComponent>();
            var asteroid = collision.transform.GetComponent<Asteroid>();
            if (healthComponent != null && asteroid != null)
            {
                healthComponent.TakeDamage(healthComponent.Health);
            }
        }

        private void OnDeath ()
        {
            OnShipDeath?.Invoke();
            Destroy(gameObject);
        }

        private void OnHealthChange (uint obj) => _powerCore.CoreState = CoreState.Stopped;

        private void Start ()
        {
            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnHealthChange += OnHealthChange;
            _healthComponent.OnDeath += OnDeath;
        }
    }
}