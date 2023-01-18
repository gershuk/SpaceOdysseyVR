using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class PowerZoneDrag : MonoBehaviour
{
    private bool busy = false;
    private Valve.VR.InteractionSystem.PowerUnit.InteractionPowerUnit activePowerUnit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other) 
    {
        var powerUnit = other.GetComponent<Valve.VR.InteractionSystem.PowerUnit.InteractionPowerUnit>();
        if(!busy && powerUnit != null) {
            busy = true;
            powerUnit.attachStanding(transform.position);
            activePowerUnit = powerUnit;
        }
    }

    void OnTriggerExit(Collider other)
    {
        var powerUnit = other.GetComponent<Valve.VR.InteractionSystem.PowerUnit.InteractionPowerUnit>();
        if(powerUnit != null && activePowerUnit == powerUnit) {
            busy = false;
            activePowerUnit.detachStanding();
            activePowerUnit = null;
        }
    }

    public Valve.VR.InteractionSystem.PowerUnit.InteractionPowerUnit GetPowerUnit() {
        return activePowerUnit;
    }

    public bool GetBusy() {
        return busy;
    }
}
