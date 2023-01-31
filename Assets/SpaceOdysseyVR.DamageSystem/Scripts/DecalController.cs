using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace SpaceOdysseyVR.DamageSystem
{
    public class DecalController : MonoBehaviour
    {
        [SerializeField]
        private DecalProjector[] _decals;

        [SerializeField]
        private HealthComponent _healthComponent;

        private int _lastNotActiveIndex;

        private void OnHealthChange (float health, float maxHealth)
        {
            if (_lastNotActiveIndex >= _decals.Length)
                return;

            var end = Math.Min((int) Math.Ceiling((maxHealth - health) / maxHealth * _decals.Length), _decals.Length - 1);
            for (var i = _lastNotActiveIndex; i <= end; i++)
            {
                _decals[i].enabled = true;
            }
            _lastNotActiveIndex = end + 1;
        }

        private void Start ()
        {
            _lastNotActiveIndex = 0;
            foreach (var decal in _decals)
            {
                decal.enabled = false;
            }

            System.Random rnd = new();
            _decals = _decals.OrderBy(x => rnd.Next()).ToArray();

            if (_healthComponent == null)
            {
                _healthComponent = GetComponent<HealthComponent>();
            }

            _healthComponent.OnHealthChange += OnHealthChange;
        }
    }
}