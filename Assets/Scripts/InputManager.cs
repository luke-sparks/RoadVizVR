using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void setViveWandControls()
    {
        IVRSystem.GetStringTrackedDeviceProperty(Prop_ManufacturerName_String)
    }

    void setOculusTouchControls()
    {

    }
}
