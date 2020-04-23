using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using VRTK;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        evaluateControllerType();
    }

    void Awake()
    {
        GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += DoButtonOnePress;
    }

    private void DoButtonOnePress(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Button one pressed");
        UIManager.Instance.openUIScreen(UIManager.UIScreens.ActionMenu, null);
    }

    public void evaluateControllerType()
    {
        SDK_BaseHeadset.HeadsetType headset = VRTK_DeviceFinder.GetHeadsetType();
        if (headset == SDK_BaseHeadset.HeadsetType.OculusRift)
            setOculusTouchControls();
        else if (headset == SDK_BaseHeadset.HeadsetType.HTCVive)
            setViveWandControls();
        else
            Debug.LogWarning("Unrecognized controller type, using default input");
    }

    private void setViveWandControls()
    {
        Debug.Log("Using Vive Wand Controllers");
    }

    private void setOculusTouchControls()
    {
        Debug.Log("Using Oculus Touch Controllers");
    } 
}
