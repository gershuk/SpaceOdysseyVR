using SpaceOdysseyVR.DamageSystem;

using TMPro;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        private HealthComponent _healthComponent;

        [SerializeField]
        private TextMeshProUGUI _text;

        private void SetValue (float health, float maxHealth) => _text.text = $"Strength {health / maxHealth * 100:F2}";

        private void Start ()
        {
            _healthComponent.OnHealthChange += SetValue;
            SetValue(_healthComponent.Health, _healthComponent.MaxHealth);
        }
    }
}