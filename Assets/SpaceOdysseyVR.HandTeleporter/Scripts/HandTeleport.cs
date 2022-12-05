#nullable enable

using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace SpaceOdysseyVR.HandTeleporter
{
    [RequireComponent(typeof(TrajectoryCalculator))]
    public sealed class HandTeleport : MonoBehaviour
    {
        private HandTeleportActionMap _actionMap;

        [SerializeField]
        private CharacterController _characterController;

        private TrajectoryCalculator _trajectoryCalculator;

        private void Awake ()
        {
            _actionMap = new();
            _actionMap.HandTeleporter.Teleport.started += StartTeleporting;
            _actionMap.HandTeleporter.Teleport.canceled += EndTeleporting;
        }

        private void EndTeleporting (CallbackContext context) => _trajectoryCalculator.EndTeleporting();

        private void OnDestroy ()
        {
            _actionMap.Dispose();
            _trajectoryCalculator.SphereReachEndPoint -= OnSphereReachEndPoint;
        }

        private void OnDisable () => _actionMap.Disable();

        private void OnEnable () => _actionMap.Enable();

        private void OnSphereReachEndPoint (Vector3 position) => TeleportateCharacter(position);

        private void Start ()
        {
            _trajectoryCalculator = GetComponent<TrajectoryCalculator>();
            _trajectoryCalculator.SphereReachEndPoint += OnSphereReachEndPoint;
            _actionMap.Enable();
        }

        private void StartTeleporting (CallbackContext context) => _trajectoryCalculator.StratTeleporting();

        private void TeleportateCharacter (Vector3 position)
        {
            _characterController.enabled = false;
            _characterController.transform.position = position;
            _characterController.enabled = true;
        }
    }
}