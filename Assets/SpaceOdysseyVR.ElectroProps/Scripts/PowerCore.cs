#nullable enable

using System;
using System.Collections;

using SpaceOdysseyVR.Reactor;

using UnityEngine;

namespace SpaceOdysseyVR.ElectroProps
{
    [RequireComponent(typeof(StatusReactor))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class PowerCore : MonoBehaviour
    {
        private bool _allCellsSetted;
        private AudioSource _audioSource;
        private CoreState _coreState = CoreState.None;

        [SerializeField]
        [Range(0.001f, 2f)]
        private float _flickerTime = 0.237f;

        private SpaceShipHull _spaceShipHull;
        private Coroutine? _startingCorutine;

        [SerializeField]
        [Range(1f, 10f)]
        private float _startingTime = 1f;

        private StatusReactor _statusReactor;

        public event Action? OnCorePowerOff;

        public event Action? OnCorePowerOn;

        public event Action<float>? OnCoreStartingProgressChange;

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

                        //case (CoreState.Stopped, not CoreState.Working, _):
                        //    Debug.LogError($"For stoping core state should be {CoreState.Working}. Current {CoreState}");
                        //    return;
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

        private void CellsStatusChanged (bool isAll)
        {
            Debug.Log($"First:{CoreState}");
            AllCellsSetted = isAll;
            CoreState = isAll ? CoreState.Starting : CoreState.Stopped;
            Debug.Log($"Second:{CoreState}");
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
            OnCorePowerOff?.Invoke();
            _audioSource.Stop();
            if (_startingCorutine != null)
            {
                StopCoroutine(_startingCorutine);
                _startingCorutine = null;
            }
            return this;
        }

        private PowerCore OnWorkingState ()
        {
            OnCorePowerOn?.Invoke();
            return this;
        }

        private void Start ()
        {
            _spaceShipHull = FindObjectOfType<SpaceShipHull>();
            _spaceShipHull.OnTakingDamage += () =>
                                             {
                                                 if (UnityEngine.Random.Range(0, 100) > 50)
                                                     _statusReactor.MakeCrash();
                                             };
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;

            _statusReactor = GetComponent<StatusReactor>();
            _statusReactor.OnStatusChanged += CellsStatusChanged;
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
                    OnCorePowerOn?.Invoke();
                }
                else
                {
                    OnCorePowerOff?.Invoke();
                }
                flickering = !flickering;
                OnCoreStartingProgressChange?.Invoke(Math.Min(1, (Time.time - startTIme) / _startingTime));
                yield return new WaitForSeconds(_flickerTime);
            }

            OnCoreStartingProgressChange?.Invoke(1);
            _startingCorutine = null;
            CoreState = CoreState.Working;
        }

        #region Context menu

        [ContextMenu(nameof(SetStartState))]
        private void SetStartState () => CoreState = CoreState.Starting;

        [ContextMenu(nameof(SetStopState))]
        private void SetStopState () => CoreState = CoreState.Stopped;

        [ContextMenu(nameof(DropAllCells))]
        public void DropAllCells () => AllCellsSetted = false;

        [ContextMenu("Init all")]
        public void InitAll ()
        {
            AllCellsSetted = true;
            CoreState = CoreState.Starting;
        }

        [ContextMenu(nameof(SetAllCells))]
        public void SetAllCells () => AllCellsSetted = true;

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