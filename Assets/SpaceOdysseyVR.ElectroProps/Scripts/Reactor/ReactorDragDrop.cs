#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.Reactor
{
    public class ReactorDragDrop : MonoBehaviour
    {
        private InteractionReactorUnit _activePowerUnit;

        private bool _busy = false;

        public event Action? OnUnitEnter;

        public event Action? OnUnitExit;

        private static float NextFloat (float min, float max)
        {
            var random = new System.Random();
            var val = (random.NextDouble() * (max - min) + min);
            return (float) val;
        }

        private void OnTriggerEnter (Collider other)
        {
            var powerUnit = other.GetComponent<InteractionReactorUnit>();
            if (!_busy && powerUnit != null)
            {
                _busy = true;
                var boxColliderPowerUnit = powerUnit.GetComponent<BoxCollider>();
                var newPositionPowerUnit = transform.position;
                newPositionPowerUnit.y = newPositionPowerUnit.y + boxColliderPowerUnit.size.y / 2;
                newPositionPowerUnit.z = newPositionPowerUnit.z - boxColliderPowerUnit.size.z;
                powerUnit.attachStanding(newPositionPowerUnit, Quaternion.Euler(0, 90, 90));
                _activePowerUnit = powerUnit;
                OnUnitEnter?.Invoke();
            }
        }

        private void OnTriggerExit (Collider other)
        {
            var powerUnit = other.GetComponent<InteractionReactorUnit>();
            if (powerUnit != null && _activePowerUnit == powerUnit)
            {
                _busy = false;
                _activePowerUnit.detachStanding();
                _activePowerUnit = null;
                OnUnitExit?.Invoke();
            }
        }

        public bool GetBusy ()
        {
            return _busy;
        }

        public InteractionReactorUnit GetPowerUnit ()
        {
            return _activePowerUnit;
        }

        public void MakeCrash ()
        {
            if (_busy)
            {
                var activePowerUnit = GetPowerUnit();
                activePowerUnit.detachStanding();
                var boxColliderPowerUnit = activePowerUnit.GetComponent<BoxCollider>();
                var newPositionPowerUnit = transform.position;
                newPositionPowerUnit.y = newPositionPowerUnit.y + boxColliderPowerUnit.size.y;
                newPositionPowerUnit.z = newPositionPowerUnit.z - boxColliderPowerUnit.size.z;
                activePowerUnit.transform.position = newPositionPowerUnit;
                var impulse = new Vector3(NextFloat(-2f, 2f), 2.0f, NextFloat(-2f, 2f));
                var activePowerUnitRigidbody = activePowerUnit.GetComponent<Rigidbody>();
                activePowerUnitRigidbody.isKinematic = false;
                activePowerUnitRigidbody.AddForce(impulse, ForceMode.Impulse);
                activePowerUnit = null;
                _busy = false;
            }
        }
    }
}