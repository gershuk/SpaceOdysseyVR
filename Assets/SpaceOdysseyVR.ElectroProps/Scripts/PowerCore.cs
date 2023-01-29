#nullable enable

using System;
using System.Collections;

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class PowerCore : MonoBehaviour
    {
        private bool _allCellsSetted;

        private AudioSource _audioSource;
        private CoreState _coreState = CoreState.None;

        [SerializeField]
        [Range(0.001f, 2f)]
        private float _flickerTime = 0.237f;

        private Coroutine? _startingCorutine;

        [SerializeField]
        [Range(1f, 10f)]
        private float _startingTime = 1f;

        public event Action? OnPowerOff;

        public event Action? OnPowerOn;

        public event Action<float>? OnStartingProgressChange;

        public bool AllCellsSetted
        {
            get => _allCellsSetted;
            set
            {
                if (_allCellsSetted == value)
                    return;

                _allCellsSetted = value;

                if (!_allCellsSetted)
                    CoreState = CoreState.Stopped;
            }
        }

        public CoreState CoreState
        {
            get => _coreState;
            set
            {
                if (_coreState == value)
                    return;

                switch (value, CoreState, _allCellsSetted)
                {
                    case (CoreState.Starting or CoreState.Working, _, false):
                        Debug.LogError($"Cells should be setted. All cells setted = {AllCellsSetted}");
                        return;

                    case (CoreState.Starting, not (CoreState.None or CoreState.Stopped), _):
                        Debug.LogError($"To start core state should be {CoreState.None} or {CoreState.Stopped}. " +
                            $"Current {CoreState}. All cells setted = {AllCellsSetted}");
                        return;

                    case (CoreState.Working, not CoreState.Starting, _):
                        Debug.LogError($"For working core state should be {CoreState.Starting}. " +
                            $"Current {CoreState}. All cells setted = {AllCellsSetted}");
                        return;

                    case (CoreState.Stopped, not CoreState.Working, _):
                        Debug.LogError($"For stoping core state should be {CoreState.Working}. Current {CoreState}");
                        return;
                }

                _coreState = value;

                _ = _coreState switch
                {
                    CoreState.None => OnNoneState(),
                    CoreState.Starting => OnStartingState(),
                    CoreState.Working => OnWorkingState(),
                    CoreState.Stopped => OnStoppedState(),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private PowerCore OnNoneState () => throw new NotImplementedException();

        private PowerCore OnStartingState ()
        {
            if (_startingCorutine != null)
            {
                Debug.LogError("Corutine already started");
                return this;
            }
            _startingCorutine = StartCoroutine(StartingCorutine());
            return this;
        }

        private PowerCore OnStoppedState ()
        {
            OnPowerOff?.Invoke();
            _audioSource.Stop();
            return this;
        }

        private PowerCore OnWorkingState ()
        {
            OnPowerOn?.Invoke();
            return this;
        }

        private void Start ()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
        }

        private IEnumerator StartingCorutine ()
        {
            var startTIme = Time.time;
            var flickering = false;
            _audioSource.Play();
            while (Time.time - startTIme < _startingTime || flickering)
            {
                if (flickering)
                {
                    OnPowerOn?.Invoke();
                }
                else
                {
                    OnPowerOff?.Invoke();
                }
                flickering = !flickering;
                OnStartingProgressChange?.Invoke(Math.Min(1, (Time.time - startTIme) / _startingTime));
                yield return new WaitForSeconds(_flickerTime);
            }

            OnStartingProgressChange?.Invoke(1);
            _startingCorutine = null;
            CoreState = CoreState.Working;
        }

        [ContextMenu(nameof(DropAllCells))]
        public bool DropAllCells () => AllCellsSetted = false;

        [ContextMenu(nameof(SetAllCells))]
        public bool SetAllCells () => AllCellsSetted = true;

        #region Context menu

        [ContextMenu(nameof(SetStartState))]
        private void SetStartState () => CoreState = CoreState.Starting;

        [ContextMenu(nameof(SetStopState))]
        private void SetStopState () => CoreState = CoreState.Stopped;

        #endregion Context menu
    }

    public enum CoreState
    {
        None = 0,
        Starting = 1,
        Working = 2,
        Stopped = 3,
    }
}