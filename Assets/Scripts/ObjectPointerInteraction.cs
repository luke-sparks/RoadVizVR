
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ObjectPointerInteraction : MonoBehaviour
{
    // private variables
    // object script is a container for the object's 
    // VRTK_InteractableObject component
    private VRTK_InteractableObject object_Script;
    // flag indicates if object is currently being touched
    private bool touch_Flag;

    // Start is called before the first frame update
    void Start()
    {
        // assign private variables
        object_Script = gameObject.GetComponent<VRTK_InteractableObject>();
        touch_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if the laser pointer touches this object
        // and the flag is not yet set,
        // call SelectObject()
        if (this.object_Script.IsTouched() && !touch_Flag)  
        {
            SelectObject();
        }
        // if the laser user stops touching this object with the laser
        // and if the flag is set,
        // call DeselectObject()
        if(!this.object_Script.IsTouched() && touch_Flag) 
        {
            DeselectObject();
        }
    }

    // selects the current object
    private void SelectObject()
    {
        Debug.Log(gameObject.name + " selected");
        // set touch flag to true so SelectObject doesn't get called
        // repeatedly
        touch_Flag = true;
    }

    // deselects the current object
    private void DeselectObject() 
    {
        touch_Flag = false;
    }
}