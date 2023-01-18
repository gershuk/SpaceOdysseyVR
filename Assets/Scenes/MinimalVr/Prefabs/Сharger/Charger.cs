using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public StatusComplete Status1;
    public StatusComplete Status2;

    private ChargerWalls chargerWalls;
    private PowerZoneDrag powerZoneDrag;

    // Start is called before the first frame update
    void Start()
    {
        chargerWalls = GetComponentInChildren<ChargerWalls>();
        powerZoneDrag = GetComponentInChildren<PowerZoneDrag>();
    }

    // Update is called once per frame
    void Update()
    {
        var isComplete = Status1.getStatusComplete() && Status2.getStatusComplete();
        chargerWalls.gameObject.SetActive(!isComplete);
        powerZoneDrag.gameObject.SetActive(isComplete);
    }

    public void dischargePowerUnit() {
        if(powerZoneDrag.GetBusy()) {
            var powerUnit = powerZoneDrag.GetPowerUnit();
            var status = powerUnit.GetComponent<PowerUnitStatus>();
            status.charged = false;
        }
    }
}
