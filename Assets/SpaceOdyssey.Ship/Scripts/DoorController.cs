using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.Ship
{
    public class DoorController : MonoBehaviour
    {
        private bool onTriggered = false;
        private float initPositionY;

        void Start ()
        {
            initPositionY = transform.position.y;
        }

        // Update is called once per frame
        void Update ()
        {
            if (onTriggered)
            {
                transform.Translate(Vector3.up * Time.deltaTime);
            }
            else
            {
                if (transform.position.y > initPositionY)
                    transform.Translate(Vector3.down * Time.deltaTime);
            }
        }

        void OnTriggerEnter (Collider other)
        {
            onTriggered = true;
        }

        void OnTriggerExit (Collider other)
        {
            onTriggered = false;
        }
    }
}