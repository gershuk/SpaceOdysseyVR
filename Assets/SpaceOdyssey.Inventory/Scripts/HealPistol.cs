using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Valve.VR;

namespace SpaceOdysseyVR.Inventory
{
    public class HealPistol : MonoBehaviour
    {
        public SteamVR_Action_Boolean action;
        public SteamVR_Input_Sources handType;

        private SpaceOdysseyVR.PlayerTools.HealRay _healRay;

        private bool isFire = false;
        // Start is called before the first frame update
        void Start ()
        {
            action.AddOnStateDownListener(TriggerDown, handType);
            action.AddOnStateUpListener(TriggerUp, handType);
            _healRay = GetComponentInChildren<SpaceOdysseyVR.PlayerTools.HealRay>();
            _healRay.gameObject.SetActive(false);
        }

        void OnDestroy() {
            action.RemoveOnStateDownListener(TriggerDown, handType);
            action.RemoveOnStateUpListener(TriggerUp, handType);
        }

        public void TriggerUp (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            _healRay.gameObject.SetActive(false);
        }
        public void TriggerDown (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            _healRay.gameObject.SetActive(true);
        }
    }
}