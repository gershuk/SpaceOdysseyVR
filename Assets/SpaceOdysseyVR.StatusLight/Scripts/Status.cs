using System;

using UnityEngine;

namespace SpaceOdysseyVR.StatusLight
{
    public class Status : MonoBehaviour
    {
        [SerializeField]
        private bool _isComplete = false;

        public event Action<bool>? OnStatusChanged;

        public bool IsComplete
        {
            get => _isComplete;
            set
            {
                _isComplete = value;
                OnStatusChanged?.Invoke(value);
            }
        }
    }
}