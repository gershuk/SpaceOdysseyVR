using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceOdysseyVR.Reactor
{
    public class StatusReactor : SpaceOdysseyVR.StatusLight.StatusComplete
    {
        private ReactorDragDrop[] reactorDragDrops;
        // Start is called before the first frame update
        void Start ()
        {
            reactorDragDrops = GetComponentsInChildren<ReactorDragDrop>();
        }

        // Update is called once per frame
        void Update ()
        {
            var countComplete = 0;
            foreach (ReactorDragDrop reactorDragDrop in reactorDragDrops)
                countComplete = countComplete + (reactorDragDrop.GetBusy() ? 1 : 0);

            SetStatusComplete((countComplete / reactorDragDrops.Length) == 1);
        }
    }
}