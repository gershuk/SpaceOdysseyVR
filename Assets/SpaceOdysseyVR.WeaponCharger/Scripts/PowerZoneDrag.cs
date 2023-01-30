using UnityEngine;

namespace SpaceOdysseyVR.WeaponCharger
{
    public class PowerZoneDrag : MonoBehaviour
    {
        private PowerUnit.InteractionPowerUnit activePowerUnit;
        private bool busy = false;

        private void OnTriggerEnter (Collider other)
        {
            var powerUnit = other.GetComponent<PowerUnit.InteractionPowerUnit>();
            if (!busy && powerUnit != null)
            {
                busy = true;
                powerUnit.attachStanding(transform.position);
                activePowerUnit = powerUnit;
            }
        }

        private void OnTriggerExit (Collider other)
        {
            var powerUnit = other.GetComponent<PowerUnit.InteractionPowerUnit>();
            if (powerUnit != null && activePowerUnit == powerUnit)
            {
                busy = false;
                activePowerUnit.detachStanding();
                activePowerUnit = null;
            }
        }

        // Start is called before the first frame update
        private void Start ()
        {
        }

        // Update is called once per frame
        private void Update ()
        {
        }

        public bool GetBusy ()
        {
            return busy;
        }

        public PowerUnit.InteractionPowerUnit GetPowerUnit ()
        {
            return activePowerUnit;
        }
    }
}