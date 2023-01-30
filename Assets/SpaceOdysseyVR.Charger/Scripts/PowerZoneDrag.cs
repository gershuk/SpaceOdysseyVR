using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace SpaceOdysseyVR.Charger
{
    public class PowerZoneDrag : MonoBehaviour
    {
        private bool busy = false;
        private PowerUnit.InteractionPowerUnit activePowerUnit;
        // Start is called before the first frame update
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        void OnTriggerEnter (Collider other)
        {
            var powerUnit = other.GetComponent<PowerUnit.InteractionPowerUnit>();
            if (!busy && powerUnit != null)
            {
                busy = true;
                powerUnit.attachStanding(transform.position);
                activePowerUnit = powerUnit;
            }
        }

        void OnTriggerExit (Collider other)
        {
            var powerUnit = other.GetComponent<PowerUnit.InteractionPowerUnit>();
            if (powerUnit != null && activePowerUnit == powerUnit)
            {
                busy = false;
                activePowerUnit.detachStanding();
                activePowerUnit = null;
            }
        }

        public PowerUnit.InteractionPowerUnit GetPowerUnit ()
        {
            return activePowerUnit;
        }

        public bool GetBusy ()
        {
            return busy;
        }
    }
}