using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Valve.VR;

namespace SpaceOdysseyVR.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        public SteamVR_Action_Boolean action;
        // a reference to the hand
        public SteamVR_Input_Sources handType;
        //reference to the sphere
        void Start ()
        {
            action.AddOnStateDownListener(TriggerDown, handType);
            action.AddOnStateUpListener(TriggerUp, handType);
            gameObject.SetActive(false);
        }
        public void TriggerUp (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            gameObject.SetActive(false);
        }
        public void TriggerDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            gameObject.SetActive(true);
        }
    }
}