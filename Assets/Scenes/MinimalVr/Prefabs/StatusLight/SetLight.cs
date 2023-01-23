using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLight : MonoBehaviour
{
    public StatusComplete statusComplete;
    private Light light;

    //-------------------------------------------------
    void Start ()
    {
        if (light == null)
        {
            light = GetComponentInChildren<Light>();
            light.color = Color.red;
        }

        if (statusComplete == null)
        {
            statusComplete = GetComponent<StatusComplete>();
        }
    }


    //-------------------------------------------------
    void Update ()
    {
        light.color = statusComplete.GetStatusComplete() ? Color.green : Color.red;
    }
}
