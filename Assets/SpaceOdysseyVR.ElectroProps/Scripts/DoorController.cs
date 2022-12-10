#nullable enable

using System;
using System.Collections;

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DoorController : MonoBehaviour
    {
        private BoxCollider _boxCollider;

        [SerializeField]
        [Range(0f, 100f)]
        private float _delta;

        [SerializeField]
        private Transform _door;

        private bool _hasPower = false;
        private bool _isOpen = false;

        private Vector3 _lockPosition;

        private Coroutine? _movingCoroutine;

        private Vector3 _openPosition;

        [SerializeField]
        [Range(0f, 100f)]
        private float _openTime;

        [SerializeField]
        private PowerCore? _powerCore;

        private Vector3 Destination => _isOpen ? _openPosition : _lockPosition;

        private IEnumerator Move ()
        {
            const float eps = 1e-5f;
            var procesTime = Vector3.Distance(_door.position, Destination)
                            / Vector3.Distance(_lockPosition, _openPosition);
            var startTime = Time.time;
            while (Vector3.Distance(_door.position, Destination) > eps)
            {
                _door.position = Vector3.Lerp(_door.position, Destination, (Time.time - startTime) / procesTime);
                yield return null;
            }
        }

        private void OnDestroy ()
        {
            if (_powerCore != null)
            {
                _powerCore.OnPowerOff -= OnPowerOff;
                _powerCore.OnPowerOn -= OnPowerOn;
            }
        }

        private void OnPowerOff ()
        {
            _hasPower = false;
            StopCoroutine();
        }

        private void OnPowerOn ()
        {
            _hasPower = true;
            _isOpen = false;
            RestartCoroutine();
        }

        private void OnTriggerEnter (Collider other)
        {
            if (_hasPower)
            {
                if (other.GetComponent<CharacterController>())
                    _isOpen = true;
                RestartCoroutine();
            }
        }

        private void OnTriggerExit (Collider other)
        {
            if (_hasPower)
            {
                if (other.GetComponent<CharacterController>())
                    _isOpen = false;
                RestartCoroutine();
            }
        }

        private void RestartCoroutine ()
        {
            StopCoroutine();
            _movingCoroutine = StartCoroutine(Move());
        }

        private void Start ()
        {
            _lockPosition = _door.localPosition;
            _openPosition = _lockPosition + Vector3.up * _delta;

            _boxCollider = GetComponent<BoxCollider>();
            _boxCollider.isTrigger = true;

            if (_powerCore == null)
                _powerCore = FindObjectOfType<PowerCore>();
            _powerCore.OnPowerOff += OnPowerOff;
            _powerCore.OnPowerOn += OnPowerOn;

            if (_powerCore.CoreState is not CoreState.Working)
                OnPowerOff();
        }

        private void StopCoroutine ()
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
        }
    }
}