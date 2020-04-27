namespace VRTK
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PointerAlternation : MonoBehaviour
    {
        // private variables:
        // pointer container for controller's pointer object,
        // events container for the controller's event handler,
        // and a flag for which pointer type is set
        private VRTK_Pointer vRTK_Pointer;
        private VRTK_ControllerEvents events;
        private int pointer_Flag;
        // Start is called before the first frame update
        void Start()
        {
            vRTK_Pointer = this.GetComponent<VRTK_Pointer>();
            events = this.GetComponent<VRTK_ControllerEvents>();
            pointer_Flag = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // user tries to teleport
            if(events.buttonOnePressed)
            {
                if (pointer_Flag == 0) {
                    set_Teleport();
                }
                if (pointer_Flag == 1) {
                    set_Select();
                }
            }   
        }

        // sets the controller's pointer to be the teleport pointer
        private void set_Teleport() 
        {
            // first, set the pointer renderer to be the bezier pointer
            vRTK_Pointer.pointerRenderer = this.GetComponent<VRTK_BezierPointerRenderer>();
            // second, set enable teleport to true
            vRTK_Pointer.enableTeleport = true;
        }

        // set the controller's pointer to be the selection pointer
        private void set_Select() 
        {
            // first, set the pointer renderer to be the straight pointer renderer
            vRTK_Pointer.pointerRenderer = this.GetComponent<VRTK_StraightPointerRenderer>();
            // second, set enable teleport to false
            vRTK_Pointer.enableTeleport = false;
        }
    }
}
