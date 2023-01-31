#nullable enable

using System;

using SpaceOdysseyVR.DamageSystem;

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(HealthComponent))]
    public abstract class AbstractProp : MonoBehaviour
    {
        [SerializeField]
        private GameObject _bad;

        [SerializeField]
        private GameObject _good;

        private HealthComponent _healthComponent;
        private bool _isPowered;

        [SerializeField]
        [Range(0f, 100f)]
        protected uint _chanceToBrokedown;

        [SerializeField]
        protected PowerCore? _powerCore;

        protected SpaceShipHull _spaceShipHull;

        public event Action<bool>? PowerStatusChanged;

        public bool IsAlive => _healthComponent.Health == _healthComponent.MaxHealth;

        public virtual bool IsPowered
        {
            get => _isPowered;
            protected set
            {
                _isPowered = value;
                if (_healthComponent.Health != _healthComponent.MaxHealth || !IsPowered)
                {
                    PowerOff();
                    PowerStatusChanged?.Invoke(false);
                }

                if (_healthComponent.Health == _healthComponent.MaxHealth && IsPowered)
                {
                    PowerOn();
                    PowerStatusChanged?.Invoke(true);
                }
            }
        }

        private void OnHealthChange (float health, float maxHealth)
        {
            if (health != maxHealth || !IsPowered)
            {
                PowerOff();
                PowerStatusChanged?.Invoke(false);
            }

            if (health == maxHealth && IsPowered)
            {
                PowerOn();
                PowerStatusChanged?.Invoke(true);
            }

            UpdateForm();
        }

        private void UpdateForm ()
        {
            if (IsAlive)
            {
                if (_good)
                    _good.SetActive(true);
                if (_bad)
                    _bad.SetActive(false);
            }
            else
            {
                if (_good)
                    _good.SetActive(false);
                if (_bad)
                    _bad.SetActive(true);
            }
        }

        protected virtual void OnPowerOff () => IsPowered = false;

        protected virtual void OnPowerOn () => IsPowered = true;

        protected virtual void OnStartingProgressChange (float obj)
        { }

        protected void OnTakingDamage ()
        {
            if ((100 - _chanceToBrokedown) < UnityEngine.Random.Range(0, 100))
                _healthComponent.Health = 0;
        }

        protected abstract void PowerOff ();

        protected abstract void PowerOn ();

        protected virtual void Start ()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _healthComponent.OnHealthChange += OnHealthChange;

            _spaceShipHull = FindObjectOfType<SpaceShipHull>();
            _spaceShipHull.OnTakingDamage += OnTakingDamage;

            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();

            _powerCore.OnCorePowerOff += OnPowerOff;
            _powerCore.OnCorePowerOn += OnPowerOn;
            _powerCore.OnCoreStartingProgressChange += OnStartingProgressChange;

            if (_powerCore.CoreState is not CoreState.Working)
            {
                PowerStatusChanged?.Invoke(false);
                OnPowerOff();
            }
            else
            {
                PowerStatusChanged?.Invoke(true);
                OnPowerOn();
            }

            UpdateForm();
        }
    }
}