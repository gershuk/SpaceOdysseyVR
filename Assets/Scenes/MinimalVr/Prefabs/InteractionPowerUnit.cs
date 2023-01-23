using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.PowerUnit
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	public class InteractionPowerUnit : Throwable
    {
        private bool isStanding = false;
        private Vector3 attachDropPosition;
        private Quaternion attachDropRotation = Quaternion.identity;


        private BoxCollider boxCollider;

        void Start() {
            boxCollider = GetComponent<BoxCollider>();
        }

        protected override void OnDetachedFromHand(Hand hand)
        {
            if(isStanding) {
                transform.position = attachDropPosition;
                transform.rotation = attachDropRotation;
                rigidbody.isKinematic = true;
            } else {
                rigidbody.isKinematic = false;
                base.OnDetachedFromHand(hand);
            }
        }

        public void attachStanding(Vector3 position) {
            isStanding = true;
            var newPosition = position;
            newPosition.y = newPosition.y - (boxCollider.size / 2).y;
            attachDropPosition = newPosition;
        }

         public void attachStanding(Vector3 position, Quaternion rotation) {
            this.attachStanding(position);
            attachDropRotation = rotation;
        }

        public void detachStanding() {
            isStanding = false;
        }
    }
}