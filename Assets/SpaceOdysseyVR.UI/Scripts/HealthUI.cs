using SpaceOdysseyVR.DamageSystem;

using TMPro;

using UnityEngine;

namespace SpaceOdysseyVR.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class HealthUI : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private bool _hasAlarm = false;

        [SerializeField]
        private HealthComponent _healthComponent;

        [SerializeField]
        private TextMeshProUGUI _text;

        private void SetValue (float health, float maxHealth) => _text.text = $"Strength {health / maxHealth * 100:F2}%";

        private void Start ()
        {
            _healthComponent.OnHealthChange += UpdateValue;
            SetValue(_healthComponent.Health, _healthComponent.MaxHealth);

            if (_hasAlarm && _audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
        }

        private void UpdateValue (float health, float maxHealth)
        {
            SetValue(health, maxHealth);
            _audioSource.Play();
        }
    }
}