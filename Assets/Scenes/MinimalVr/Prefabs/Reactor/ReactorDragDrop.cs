using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class ReactorDragDrop : MonoBehaviour
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
            var boxColliderPowerUnit = powerUnit.GetComponent<BoxCollider>();
            var newPositionPowerUnit = transform.position;
            newPositionPowerUnit.y = newPositionPowerUnit.y + boxColliderPowerUnit.size.y / 2;
            newPositionPowerUnit.x = newPositionPowerUnit.x + boxColliderPowerUnit.size.x;
            powerUnit.attachStanding(newPositionPowerUnit, Quaternion.Euler(0, 0, 90));
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
