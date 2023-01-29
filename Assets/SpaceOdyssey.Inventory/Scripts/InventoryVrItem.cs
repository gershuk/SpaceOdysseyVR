using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.Inventory
{
    public class InventoryVrItem : MonoBehaviour
    {
        public GameObject prefab;
        private GameObject spawnedItem;
        public Valve.VR.InteractionSystem.Hand.AttachmentFlags attachmentFlags = Valve.VR.InteractionSystem.Hand.defaultAttachmentFlags;
        // Start is called before the first frame update
        void Start ()
        {

        }

        // Update is called once per frame
        void Update ()
        {

        }

        public void AttachVrItemToHand (Valve.VR.InteractionSystem.Hand hand)
        {
            if (prefab != null)
            {
                spawnedItem = GameObject.Instantiate(prefab);
                spawnedItem.SetActive(true);
                hand.AttachObject(spawnedItem, Valve.VR.InteractionSystem.GrabTypes.None, attachmentFlags);
            }
            else
            {
                for (int i = 0; i < hand.AttachedObjects.Count; i++)
                {
                    GameObject detachedItem = hand.AttachedObjects[i].attachedObject;
                    hand.DetachObject(detachedItem);
                }
            }
        }
    }
}