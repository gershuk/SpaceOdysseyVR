using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace SpaceOdysseyVR.Charger
{
    public class Charger : MonoBehaviour
    {
        public StatusComplete[] Statuses;

        private ChargerWalls chargerWalls;
        private PowerZoneDrag powerZoneDrag;

        // Start is called before the first frame update
        void Start ()
        {
            chargerWalls = GetComponentInChildren<ChargerWalls>();
            powerZoneDrag = GetComponentInChildren<PowerZoneDrag>();
        }

        // Update is called once per frame
        void Update ()
        {
            var isComplete = true;
            foreach (StatusComplete Status in Statuses)
                isComplete = isComplete && Status.GetStatusComplete();

            chargerWalls.gameObject.SetActive(!isComplete);
            powerZoneDrag.gameObject.SetActive(isComplete);
        }

        public void dischargePowerUnit ()
        {
            if (powerZoneDrag.GetBusy())
            {
                var powerUnit = powerZoneDrag.GetPowerUnit();
                var status = powerUnit.GetComponent<PowerUnitStatus>();
                status.charged = false;
            }
        }

        public bool isCharged ()
        {
            if (powerZoneDrag.GetBusy())
            {
                var powerUnit = powerZoneDrag.GetPowerUnit();
                var status = powerUnit.GetComponent<PowerUnitStatus>();
                return status.charged;
            }
            return false;
        }
    }

}
