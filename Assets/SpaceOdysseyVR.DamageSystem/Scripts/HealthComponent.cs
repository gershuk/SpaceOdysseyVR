#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.DamageSystem
{
    public sealed class HealthComponent : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 1000)]
        private uint _health = 100;

        [SerializeField]
        [Range(0, 1000)]
        private uint _maxHealth = 100;

        public event Action? OnDeath;

        public event Action<uint>? OnHealthChange;

        public uint Health
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
                _health = value;
                OnHealthChange?.Invoke(value);

                if (_health == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }

        public uint MaxHealth { get => _maxHealth; set => _maxHealth = value; }

        [ContextMenu(nameof(FullHeal))]
        public void FullHeal () => Health = MaxHealth;

        public void TakeDamage (uint damage) => Health = Math.Max(0, Health - damage);

        public void TakeHeal (uint heal) => Health = Math.Min(MaxHealth, Health + heal);
    }
}