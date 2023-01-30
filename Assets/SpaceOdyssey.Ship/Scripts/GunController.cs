using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Valve.VR;

namespace SpaceOdysseyVR.Ship
{
    public class GunController : MonoBehaviour
    {
        public Valve.VR.InteractionSystem.Player player;
        public Valve.VR.InteractionSystem.Player shipGunPlayer;

        public SteamVR_Action_Boolean actionFire;
        public SteamVR_Input_Sources handTypeFire;
        public SteamVR_Action_Boolean actinExit;
        public SteamVR_Input_Sources handTypeExit;
        // a reference to the hand
        public WeaponSystem.WeaponSystem weaponSystem;

        // Start is called before the first frame update
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void ActivatedGun ()
        {
            player.gameObject.SetActive(false);
            shipGunPlayer.gameObject.SetActive(true);
            actionFire.AddOnStateUpListener(Fire, handTypeFire);
            actinExit.AddOnStateUpListener(Exit, handTypeExit);
        }

        public void ActivatedPlayer ()
        {
            shipGunPlayer.gameObject.SetActive(false);
            player.gameObject.SetActive(true);
            
            actionFire.RemoveOnStateUpListener(Fire, handTypeFire);
            actionFire.RemoveOnStateUpListener(Exit, handTypeExit);
        }

        private void Fire (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            weaponSystem.Shoot();
        }

        private void Exit (SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            ActivatedPlayer();
        }
    }
}