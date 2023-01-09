using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusComplete : Valve.VR.InteractionSystem.LinearMapping
{
    private bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       isComplete = value == 1;
    }

    public bool getStatusComplete() {
        return isComplete;
    }
}
