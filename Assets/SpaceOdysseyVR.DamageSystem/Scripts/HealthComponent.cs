#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.DamageSystem
{
    public sealed class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1000)]
        private float _health = 100;

        [SerializeField]
        [Range(0, 1000)]
        private float _maxHealth = 100;

        public event Action? OnDeath;

        public event Action<float, float>? OnHealthChange;

        public float Health
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
                _health = value;
                OnHealthChange?.Invoke(Health, MaxHealth);

                if (_health == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }

        [ContextMenu(nameof(FullHeal))]
        public void FullHeal () => Health = MaxHealth;

        public void TakeDamage (float damage) => Health = Math.Max(0, Health - damage);

        public void TakeHeal (float heal) => Health = Math.Min(MaxHealth, Health + heal);
    }
}