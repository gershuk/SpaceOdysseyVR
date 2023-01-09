using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUnitStatus : MonoBehaviour
{
    public bool charged = false;
    public GameObject EmptyIndicator;
    public GameObject FullIndicator;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        EmptyIndicator.SetActive(!charged);
        FullIndicator.SetActive(charged);
    }
}
