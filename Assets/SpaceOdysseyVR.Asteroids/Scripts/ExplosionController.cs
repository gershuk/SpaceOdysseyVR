#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.Asteroids
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField]
        [Range(MinStrenght, MaxStrenght)]
        private float _strength = 1;

        [SerializeField]
        [Range(1f, 2.5f)]
        private float _timeout = 2.5f;

        public const float MaxStrenght = 10f;
        public const float MinStrenght = 1f;

        public float Strength
        {
            get => _strength;
            set
            {
                if (_strength is > MaxStrenght or < MinStrenght)
                    throw new ArgumentOutOfRangeException($"Strength should be > {MinStrenght} and < {MaxStrenght}");

                _strength = value;
            }
        }

        private void Start ()
        {
            transform.localScale = new(Strength, Strength, Strength);
            Destroy(gameObject, _timeout);
        }
    }
}