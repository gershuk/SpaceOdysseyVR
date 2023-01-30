using UnityEngine;

namespace SpaceOdysseyVR.WeaponCharger
{
    public class Charger : MonoBehaviour
    {
        private ChargerWalls _chargerWalls;
        private PowerZoneDrag _powerZoneDrag;
        public StatusComplete[] Statuses;

        // Start is called before the first frame update
        private void Start ()
        {
            _chargerWalls = GetComponentInChildren<ChargerWalls>();
            _powerZoneDrag = GetComponentInChildren<PowerZoneDrag>();
        }

        // Update is called once per frame
        private void Update ()
        {
            var isComplete = true;
            foreach (var Status in Statuses)
                isComplete = isComplete && Status.GetStatusComplete();

            _chargerWalls.gameObject.SetActive(!isComplete);
            _powerZoneDrag.gameObject.SetActive(isComplete);
        }

        public void DischargePowerUnit ()
        {
            if (_powerZoneDrag.GetBusy())
            {
                var powerUnit = _powerZoneDrag.GetPowerUnit();
                var status = powerUnit.GetComponent<PowerUnitStatus>();
                status.Discharge();
            }
        }

        public bool IsCharged ()
        {
            if (_powerZoneDrag.GetBusy())
            {
                var powerUnit = _powerZoneDrag.GetPowerUnit();
                var status = powerUnit.GetComponent<PowerUnitStatus>();
                return status.Charged;
            }
            return false;
        }
    }
}