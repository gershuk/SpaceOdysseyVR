using System;

using UnityEngine;

namespace SpaceOdysseyVR.WeaponCharger
{
    public class PowerUnitStatus : MonoBehaviour
    {
        [SerializeField]
        private uint _charge = 5;

        public GameObject EmptyIndicator;
        public GameObject FullIndicator;

        private uint Charge
        {
            get => _charge;
            set
            {
                if (_charge != value)
                {
                    _charge = value;
                    UpdateBody();
                }
            }
        }

        public bool Charged => Charge > 0;

        private void Start ()
        {
            UpdateBody();
        }

        private void UpdateBody ()
        {
            EmptyIndicator.SetActive(!Charged);
            FullIndicator.SetActive(Charged);
        }

        public void Discharge ()
        {
            Charge = Math.Max(0, Charge - 1);
        }
    }
}