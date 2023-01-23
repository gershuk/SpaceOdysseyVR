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

    static float NextFloat(float min, float max){
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }

    public void MakeCrash() {
        var isBusy = GetBusy();
        if(isBusy) {
            var activePowerUnit = GetPowerUnit();
            activePowerUnit.detachStanding();
            var impulse = new Vector3(0.0f, 2.0f, NextFloat(-1f, 1f));
            var activePowerUnitRigidbody = activePowerUnit.GetComponent<Rigidbody>();
            activePowerUnitRigidbody.isKinematic = false;
            activePowerUnitRigidbody.AddForce(impulse, ForceMode.Impulse);            
            activePowerUnit = null;
            busy = false;
        }
    }
}
